using Fitradar.Application.Common;
using System;

namespace Fitradar.Application.UseCases.Reactions.OutboundEvents;

/// <summary>
/// Outbound integration event published when a comment is added.
/// Carries stable facts already known at command time so the Azure Function
/// consumer can fetch only the volatile host-notification data it needs.
/// </summary>
/// <param name="CommentId">The unique identifier of the added comment</param>
/// <param name="PostedById">The user who posted the comment</param>
/// <param name="SportEventInstancePublicId">The public id of the sport event instance the comment belongs to</param>
public record CommentAdded(
    Guid CommentId,
    string PostedById,
    Guid SportEventInstancePublicId) : IOutboundIntegrationEvent;
