using Fitradar.Application.Persistence.ReadStores.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Application.Persistence.ReadStores;

public interface ICommentReadStore
{
    Task<Guid> CreateAsync(CommentReadModel comment, CancellationToken cancellationToken = default);
}
