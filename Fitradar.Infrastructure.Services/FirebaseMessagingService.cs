using CSharpFunctionalExtensions;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Fitradar.Application.Contracts.Integration.Services.Config;
using Fitradar.Application.Services;
using Fitradar.Application.Services.Dto;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Infrastructure.Services;

// Examples on how to use Firebase Admin SDK can be found here https://github.com/firebase/firebase-admin-dotnet
public class FirebaseMessagingService : IPushMessagingService
{
    private readonly ILogger<FirebaseMessagingService> _logger;
    private readonly FirebaseMessaging _messaging;
    private readonly bool _dryRun;

    // FCM batch limit is 500 messages per request
    private const int FCM_BATCH_SIZE = 500;

    // Maximum concurrent batch operations
    private const int MAX_PARALLEL_BATCHES = 10;

    // We send only data push messages to Android to let the Android Application treat the push
    // in the same way for cases when the application is in foreground and when the application is closed
    // or in the background

    public FirebaseMessagingService(
        IOptionsMonitor<FirebaseClientOptions> optionsMonitor,
        ILoggerFactory loggerFactory)
    {
        var options = optionsMonitor.CurrentValue;
        _logger = loggerFactory.CreateLogger<FirebaseMessagingService>();
        _dryRun = options.OnlyValidate;

        if (FirebaseApp.DefaultInstance == null)
        {
            var firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJsonParameters(options)
            });
            if (firebaseApp == null)
            {
                _logger.LogError("Firebase Application instance was not created");
            }
        }

        _messaging = FirebaseMessaging.DefaultInstance;
        if (_messaging == null)
        {
            _logger.LogError("Firebase Messaging default instance was not created");
        }
    }

    // The method should be called when
    // 1) New sport event is created
    // 2) Sport event is updated,
    // 3) Sport event is cancelled,
    // 4) Only one seat is left on Sport Event
    // 5) Someone booked a seat on Sport Event,
    // 6) Someone started to follow Sport Event,
    // 7) Someone liked Sport Event
    // 8) Someone commented on Sport Event
    // 9) Someone left a feedback on Sport Event
    // 10) someone started to follow you
    // 11) you got a new visible rating
    // 12) your Stripe account was approved
    public async Task<Result<PushMessageResult>> SendPushMessage(
        PushMessage message,
        CancellationToken cancellationToken = default)
    {
        EnsureMessagingInstanceIsCreated();

        var androidData = new AndroidConfig()
        {
            Data = new Dictionary<string, string>
            {
                { "title", message.Title },
                { "body", message.Body },
                { "fitradar_notification_type", message.EntityName },
                { "fitradar_event_id", message.EventId },
                { "fitradar_user_id", message.UserId },
                { "navigation_uri", message.NavigationLink }
            }
        };

        var appleData = new ApnsConfig()
        {
            Aps = new Aps()
            {
                Alert = new ApsAlert()
                {
                    Title = message.Title,
                    Body = message.Body,
                }
            },
            CustomData = new Dictionary<string, object>
            {
                { "fitradar_notification_type", message.EntityName },
                { "fitradar_event_id", message.EventId },
                { "fitradar_user_id", message.UserId },
                { "navigation_uri", message.NavigationLink }
            }
        };

        // Local bag: each call is isolated — no shared mutable state between concurrent invocations
        var unregisteredTokens = new ConcurrentBag<string>();

        if (message.Receivers.Count == 1)
        {
            await SendMessage(message.Receivers[0], androidData, appleData, unregisteredTokens, cancellationToken);
        }
        else
        {
            await SendMulticastMessage(message.Receivers, androidData, appleData, unregisteredTokens, cancellationToken);
        }

        return Result.Success(new PushMessageResult
        {
            UnregisteredFcmTokens = [.. unregisteredTokens]
        });
    }

    private void EnsureMessagingInstanceIsCreated()
    {
        if (_messaging != null)
        {
            return;
        }
        _logger.LogError("Firebase Messaging default instance is null");
        throw new InvalidOperationException("Firebase Messaging was not initialized. See application logs.");
    }

    private async Task SendMessage(
        PushReceiver receiver,
        AndroidConfig androidData,
        ApnsConfig appleData,
        ConcurrentBag<string> unregisteredTokens,
        CancellationToken cancellationToken = default)
    {
        if (receiver.FcmTokens.Count == 0)
        {
            _logger.LogWarning("User {Username} doesn't have any FCM token yet. We can't send push notification!", receiver.Username);
            return;
        }

        try
        {
            if (receiver.FcmTokens.Count == 1)
            {
                var message = new Message()
                {
                    Token = receiver.FcmTokens[0],
                    Android = androidData,
                    Apns = appleData
                };

                var messageId = await _messaging.SendAsync(message, _dryRun, cancellationToken);
                _logger.LogInformation("FCM message {Title} for user {Username} was sent with push ID {MessageId}",
                    appleData.Aps.Alert.Title, receiver.Username, messageId);
            }
            else
            {
                var multicastMessage = new MulticastMessage()
                {
                    Tokens = receiver.FcmTokens.ToList(),
                    Android = androidData,
                    Apns = appleData
                };
                var batchResponse = await _messaging.SendEachForMulticastAsync(multicastMessage, _dryRun, cancellationToken);
                LogFcmSendResult(batchResponse, receiver.FcmTokens, appleData.Aps.Alert.Title, unregisteredTokens);
            }
        }
        catch (FirebaseMessagingException e)
        {
            LogFcmException(e, receiver.Username, receiver.FcmTokens, appleData.Aps.Alert.Title);
        }
    }

    private async Task SendMulticastMessage(
        IReadOnlyList<PushReceiver> receivers,
        AndroidConfig androidData,
        ApnsConfig appleData,
        ConcurrentBag<string> unregisteredTokens,
        CancellationToken cancellationToken = default)
    {
        var allMessages = new List<Message>();

        foreach (var receiver in receivers)
        {
            if (receiver.FcmTokens.Count == 0)
            {
                _logger.LogWarning("User {Username} doesn't have any FCM tokens. It looks like the user is not logged in any device. We skip the push notification.", receiver.Username);
                continue;
            }

            foreach (var fcmToken in receiver.FcmTokens.Where(t => !string.IsNullOrWhiteSpace(t)))
            {
                allMessages.Add(new Message()
                {
                    Token = fcmToken,
                    Android = androidData,
                    Apns = appleData
                });
            }
        }

        if (allMessages.Count == 0)
        {
            _logger.LogWarning("No valid FCM tokens found among {ReceiverCount} receivers", receivers.Count);
            return;
        }

        _logger.LogInformation("Preparing to send FCM message to {TokenCount} devices from {ReceiverCount} receivers",
            allMessages.Count, receivers.Count);

        var batches = allMessages.Chunk(FCM_BATCH_SIZE).ToList();

        _logger.LogInformation("Split into {BatchCount} batches for processing", batches.Count);

        var semaphore = new SemaphoreSlim(MAX_PARALLEL_BATCHES);
        var tasks = batches.Select(async batch =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                await SendBatchAsync(batch, appleData.Aps.Alert.Title, unregisteredTokens, cancellationToken);
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);

        _logger.LogInformation("Completed sending FCM message to all batches. Unregistered tokens: {Count}",
            unregisteredTokens.Count);
    }

    private void LogFcmSendResult(
        BatchResponse response,
        IReadOnlyList<string> fcmTokens,
        string msgTitle,
        ConcurrentBag<string> unregisteredTokens)
    {
        if (response is null)
        {
            _logger.LogError("Null batch response received for FCM message {Title}", msgTitle);
            return;
        }

        if (response.FailureCount == 0)
        {
            _logger.LogInformation("FCM message {Title} was successfully sent to {SuccessCount} devices", msgTitle, response.SuccessCount);
            return;
        }

        _logger.LogError("{SuccessCount} FCM messages delivered successfully and {FailureCount} failed for {Title}",
            response.SuccessCount, response.FailureCount, msgTitle);

        for (int i = 0; i < response.Responses.Count; i++)
        {
            var rsp = response.Responses[i];
            if (rsp.IsSuccess)
            {
                continue;
            }

            switch (rsp.Exception.MessagingErrorCode)
            {
                case MessagingErrorCode.Unregistered:
                    _logger.LogError("FCM token {FcmToken} is unregistered and should be deleted", fcmTokens[i]);
                    unregisteredTokens.Add(fcmTokens[i]);
                    break;
                case MessagingErrorCode.Unavailable:
                    _logger.LogError("FCM message delivery failed: FCM web service is temporarily unavailable");
                    break;
                case MessagingErrorCode.QuotaExceeded:
                    _logger.LogError("FCM message delivery failed: sending limit exceeded for the message target");
                    break;
                default:
                    _logger.LogError("FCM message delivery failed with error code {ErrorCode}", rsp.Exception.MessagingErrorCode);
                    break;
            }
        }
    }

#pragma warning disable CS0618 // Type or member is obsolete
    private async Task SendBatchAsync(
        Message[] messages,
        string msgTitle,
        ConcurrentBag<string> unregisteredTokens,
        CancellationToken cancellationToken)
    {
        try
        {
            // Use deprecated SendAllAsync for optimal performance (single RPC call per batch)
            // Despite being deprecated, this is significantly more efficient for high-volume scenarios
            // SendAllAsync: 500 messages = 1 HTTP call vs SendEachAsync: 500 messages = 500 HTTP calls
            var batchResponse = await _messaging.SendAllAsync(messages, _dryRun, cancellationToken);
            var tokens = messages.Select(m => m.Token).ToArray();
            LogFcmSendResult(batchResponse, tokens, msgTitle, unregisteredTokens);
        }
        catch (FirebaseMessagingException e)
        {
            _logger.LogError(e, "Firebase messaging exception while sending batch of {Count} messages with title {Title}",
                messages.Length, msgTitle);
            _logger.LogError("FCM error code: {ErrorCode}", e.MessagingErrorCode);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while sending batch of {Count} messages", messages.Length);
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete

    private void LogFcmException(
        FirebaseMessagingException e,
        string username,
        IReadOnlyList<string> affectedTokens,
        string msgTitle)
    {
        _logger.LogError("FCM message {Title} was not delivered to user {Username}", msgTitle, username);
        switch (e.MessagingErrorCode)
        {
            case MessagingErrorCode.Unregistered:
                // A transport-level exception is not per-token granular — log all affected tokens.
                // Per-token precision is only available through BatchResponse in LogFcmSendResult.
                foreach (var token in affectedTokens)
                {
                    _logger.LogError("FCM token {FcmToken} may be unregistered and should be reviewed", token);
                }
                break;
            case MessagingErrorCode.Unavailable:
                _logger.LogError("FCM web service is temporarily unavailable");
                break;
            case MessagingErrorCode.QuotaExceeded:
                _logger.LogError("FCM sending limit exceeded for the message target");
                break;
            default:
                _logger.LogError("FCM push failed with error code {ErrorCode}", e.MessagingErrorCode);
                break;
        }
    }
}
