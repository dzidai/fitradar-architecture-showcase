namespace Fitradar.Application.Common.Infrastructure
{
    public interface IHealthReporter
    {
        Task<string> GetHealthStatusAsync(CancellationToken cancellationToken = default);
    }
}
