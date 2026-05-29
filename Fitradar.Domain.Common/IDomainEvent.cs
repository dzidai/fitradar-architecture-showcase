using MediatR;
using System;

namespace Fitradar.Domain.Events
{
    public interface IDomainEvent : INotification
    {
        //Guid SourceId { get; }
    }
}
