using Fitradar.Application.Persistence.ReadStores.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Application.Persistence.ReadStores;

public interface IInboxReadStore
{
    Task UpsertAsync(InboxMessageReadModel message, CancellationToken cancellationToken = default);
}
