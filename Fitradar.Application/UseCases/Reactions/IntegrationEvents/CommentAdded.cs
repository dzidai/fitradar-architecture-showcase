using Fitradar.Application.Common;
using Fitradar.Application.Notifications;
using Fitradar.Application.Persistence.ReadStores;
using Fitradar.Application.Persistence.ReadStores.Models;
using Fitradar.Application.Services;
using Fitradar.Application.Services.Dto;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Application.UseCases.Reactions.IntegrationEvents;

public sealed class CommentAdded : IIntegrationEvent
{
    public Guid CommentId { get; init; }
    public required string PostedById { get; init; }
    public Guid SportEventInstancePublicId { get; init; }
    public required string HostId { get; init; }
    public required string HostUserName { get; init; }
    public Guid SportEventPublicId { get; init; }
    public Guid SportEventId { get; init; }
    public required string[] HostFcmTokens { get; init; }


    public class NotifySportEventHostByPush : IIntegrationEventHandler<CommentAdded>
    {
        private readonly IPushMessagingService _notificationService;
        private readonly IPublisher _publisher;
        private readonly ILogger<NotifySportEventHostByPush> _logger;

        public NotifySportEventHostByPush(
            IPushMessagingService notificationService,
            IPublisher publisher,
            ILogger<NotifySportEventHostByPush> logger)
        {
            _notificationService = notificationService;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task Handle(CommentAdded notification, CancellationToken cancellationToken)
        {
            // Don't send push notification if user commented on their own event
            if (notification.HostId == notification.PostedById)
            {
                return;
            }

            var pushMessage = new PushMessage
            {
                Receivers =
                [
                    new PushReceiver
                    {
                        Id = notification.HostId,
                        Username = notification.HostUserName,
                        FcmTokens = notification.HostFcmTokens
                    }
                ],
                Title = "New Comment",
                Body = "New comment was added on your sport event",
                EntityName = "event",
                EventId = notification.SportEventPublicId.ToString()
            };

            var result = await _notificationService.SendPushMessage(pushMessage, cancellationToken);

            _logger.LogInformation("Push notification sent to sport event organizer {OrganizerId} for comment {CommentId}",
                notification.HostId, notification.CommentId);

            if (result.IsSuccess && result.Value.UnregisteredFcmTokens.Count > 0)
            {
                await _publisher.Publish(
                    new FcmTokensExpired { FcmTokens = result.Value.UnregisteredFcmTokens },
                    cancellationToken);
            }
        }
    }


    public class NotifySportEventHostInInbox : IIntegrationEventHandler<CommentAdded>
    {
        private const string WorkoutNavUrl = "fitradar://event?id={0}";

        private readonly IInboxReadStore _inboxReadStore;
        private readonly ILogger<NotifySportEventHostInInbox> _logger;

        public NotifySportEventHostInInbox(
            IInboxReadStore inboxReadStore,
            ILogger<NotifySportEventHostInInbox> logger)
        {
            _inboxReadStore = inboxReadStore;
            _logger = logger;
        }

        public async Task Handle(CommentAdded notification, CancellationToken cancellationToken)
        {
            // Don't create inbox message if user commented on their own event
            if (notification.HostId == notification.PostedById)
            {
                return;
            }

            var inboxMessage = new InboxMessageReadModel
            {
                ReceiverId = notification.HostId,
                NavigationLink = string.Format(WorkoutNavUrl, notification.SportEventPublicId),
                Source = MessageSource.Comment,
                TriggeredById = notification.PostedById,
                SportEventPublicId = notification.SportEventPublicId,
                AvatarId = notification.SportEventId.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            await _inboxReadStore.UpsertAsync(inboxMessage, cancellationToken);

            _logger.LogInformation("Inbox message created for sport event host {hostId} for comment {commentId}",
                notification.HostId, notification.CommentId);
        }
    }
}
