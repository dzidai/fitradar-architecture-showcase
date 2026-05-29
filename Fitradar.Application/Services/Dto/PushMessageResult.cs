using System.Collections.Generic;

namespace Fitradar.Application.Services.Dto;

public sealed record PushMessageResult
{
    public static readonly PushMessageResult Empty = new();

    public IReadOnlyList<string> UnregisteredFcmTokens { get; init; } = [];
}
