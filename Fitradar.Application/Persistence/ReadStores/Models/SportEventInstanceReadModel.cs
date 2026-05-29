using System;

namespace Fitradar.Application.Persistence.ReadStores.Models;

/// <summary>
/// Read-side projection. Rebuilt from domain events.
/// Lives in Application layer, not Domain.
/// </summary>
public sealed class SportEventInstanceReadModel
{
    public long Id { get; init; }
    public Guid PublicId { get; init; }
    public Guid SportEventId { get; init; }

    // ── Presentation-only formatting ──────────────────────────────────
    public long StartTimeMillis { get; init; }
    public long EndTimeMillis { get; init; }
    public int StartsInHours { get; init; }

    // ── Engagement counters (social read model) ───────────────────────
    public int NumberOfLikes { get; init; }
    public int NumberOfComments { get; init; }
    public int NumberOfFollowers { get; init; }
    public int NumberOfPromoters { get; init; }

    // ── Derived ticket/complaint data ─────────────────────────────────
    public int NumberOfBookedTickets { get; init; }
    public int NumberOfLeftTickets { get; init; }   // tickets - booked
    public int NumberOfSoldAndUndisputedTickets { get; init; }

    // ── UI ────────────────────────────────────────────────────────────
    public bool HasUniqueCover { get; init; }
    public bool IsCancelled { get; init; }
    public bool IsArchived { get; init; }   // persistence artifact
}
