using System;

namespace Fitradar.Application.Behaviors;

public interface ISportEventInteractionCommand
{
    Guid SportEventInstancePublicId { get; set; }
}
