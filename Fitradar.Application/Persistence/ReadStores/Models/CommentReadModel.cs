using System;

namespace Fitradar.Application.Persistence.ReadStores.Models;

public sealed record CommentReadModel
{
    public required string CommentText { get; init; }

    public required string PostedById { get; init; }

    public required Guid SportEventInstancePublicId { get; init; }

    public required DateTime PostedAt { get; init; }
}
