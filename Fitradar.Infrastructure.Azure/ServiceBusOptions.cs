using System.Collections.Generic;

namespace Fitradar.Infrastructure.Azure
{
    public class ServiceBusOptions
    {
        public string? ConnectionString { get; set; }

        // Key = logical route key, Value = queue/topic name.
        // If not provided for a key, ServiceBusQueue falls back to the key itself.
        public Dictionary<string, string> RouteMap { get; set; } = new();
    }
}
