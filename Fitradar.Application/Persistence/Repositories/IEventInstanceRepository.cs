using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Application.Persistence.Repositories;

public interface IEventInstanceRepository
{
    Task<bool> ExistsAsync(Guid publicId, CancellationToken cancellationToken = default);
}
