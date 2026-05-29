using Fitradar.Application.Persistence.Dao;
using Fitradar.Application.UseCases.Reactions.IntegrationEvents;
using Fitradar.Infrastructure.Azure;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using OutboundCommentAdded = Fitradar.Application.UseCases.Reactions.OutboundEvents.CommentAdded;

namespace Fitradar.Integration.Functions.Reactions
{
    public class OnCommentAddedFunction
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ISportEventNotificationDao _sportEventNotificationDao;

        public OnCommentAddedFunction(
            ILoggerFactory loggerFactory,
            IMediator mediator,
            ISportEventNotificationDao sportEventNotificationDao)
        {
            _mediator = mediator;
            _sportEventNotificationDao = sportEventNotificationDao;
            _logger = loggerFactory.CreateLogger<OnCommentAddedFunction>();
        }

        [Function("OnCommentAdded")]
        public async Task Run(
            [ServiceBusTrigger(ServiceBusQueue.CommentsQueueName, Connection = "ServiceBus:ConnectionString")]
            string message)
        {
            var outboundEvent = JsonSerializer.Deserialize<OutboundCommentAdded>(message);
            if (outboundEvent == null)
            {
                _logger.LogError("Failed to deserialize CommentAdded message: {message}", message);
                return;
            }

            var hostData = await _sportEventNotificationDao.GetHostNotificationDataAsync(
                outboundEvent.SportEventInstancePublicId);

            if (hostData == null)
            {
                _logger.LogWarning(
                    "SportEventInstance {publicId} not found for comment {commentId}. Further processing is stopped",
                    outboundEvent.SportEventInstancePublicId, outboundEvent.CommentId);
                return;
            }

            await _mediator.Publish(new CommentAdded
            {
                CommentId = outboundEvent.CommentId,
                PostedById = outboundEvent.PostedById,
                SportEventInstancePublicId = outboundEvent.SportEventInstancePublicId,
                HostId = hostData.HostId,
                HostUserName = hostData.HostUserName,
                SportEventPublicId = hostData.SportEventPublicId,
                SportEventId = hostData.SportEventId,
                HostFcmTokens = hostData.HostFcmTokens
            });
        }
    }
}
