using System.Collections.Generic;

namespace Fitradar.Application.Services.Dto;

public sealed record PushMessage
{
    public required IReadOnlyList<PushReceiver> Receivers { get; init; }

    public required string Title { get; init; }

    public required string EntityName { get; init; }

    public required string Body { get; init; }

    public string? UserId { get; init; }

    public string? EventId { get; init; }

    public string? NavigationLink { get; init; }
}
