using Azure.Messaging.ServiceBus;
using Fitradar.Application.Common.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fitradar.Infrastructure.Azure
{
    public class ServiceBusHealthReporter : IHealthReporter
    {
        private const string PingQueueName = "healthcheck";
        private const string PingMessage = "Testing Azure Service Bus health status";

        private readonly ServiceBusOptions _options;
        private readonly ILogger<ServiceBusHealthReporter> _logger;

        public ServiceBusHealthReporter(
            IOptions<ServiceBusOptions> options,
            ILogger<ServiceBusHealthReporter> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<string> GetHealthStatusAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await using var client = new ServiceBusClient(_options.ConnectionString);
                await using var sender = client.CreateSender(PingQueueName);
                await sender.SendMessageAsync(new ServiceBusMessage(PingMessage), cancellationToken);

                await using var receiver = client.CreateReceiver(PingQueueName);
                var received = await receiver.ReceiveMessageAsync(cancellationToken: cancellationToken);
                if (received is not null)
                {
                    await receiver.CompleteMessageAsync(received, cancellationToken);
                    return "OK";
                }

                _logger.LogError("Azure Service Bus was not able to send and receive a message");
                return "Failed";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during Azure Service Bus operations: {Message}", ex.Message);
                return "Failed";
            }
        }
    }
}
