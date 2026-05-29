using Fitradar.Application.Contracts.Integration.Services.Config;
using Fitradar.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fitradar.Infrastructure.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
        {
            services.AddSingleton<IPushMessagingService, FirebaseMessagingService>();
            services.AddOptions<FirebaseClientOptions>().BindConfiguration("Firebase");

            return services;
        }
    }
}
