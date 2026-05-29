using Fitradar.Application.Notifications;
using System;

namespace Fitradar.Application.Persistence.ReadStores.Models;

public sealed record InboxMessageReadModel
{
    public required string ReceiverId { get; init; }

    public required string NavigationLink { get; init; }

    public required MessageSource Source { get; init; }

    public required string TriggeredById { get; init; }

    public Guid? SportEventPublicId { get; init; }

    public required string AvatarId { get; init; }

    public required DateTime CreatedAt { get; init; }
}
