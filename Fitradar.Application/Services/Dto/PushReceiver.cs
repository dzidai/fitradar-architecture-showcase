using System.Collections.Generic;

namespace Fitradar.Application.Services.Dto;

public sealed record PushReceiver
{
    public required string Id { get; init; }

    public required IReadOnlyList<string> FcmTokens { get; init; }

    public required string Username { get; init; }
}
