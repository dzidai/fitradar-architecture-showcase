using Azure.Messaging.ServiceBus;
using Fitradar.Application.Common.Infrastructure;
using Fitradar.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fitradar.Infrastructure.Azure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAzureInfrastructure(this IServiceCollection services)
        {
            services.AddOptions<ServiceBusOptions>().BindConfiguration("ServiceBus");

            services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value;
                return new ServiceBusClient(options.ConnectionString);
            });

            services.AddSingleton<IServiceBusQueue, ServiceBusQueue>();
            services.AddSingleton<IHealthReporter, ServiceBusHealthReporter>();

            return services;
        }
    }
}
