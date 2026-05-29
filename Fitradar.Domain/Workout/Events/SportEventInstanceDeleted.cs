using Fitradar.Domain.Common;

namespace Fitradar.Domain.Workout.Events
{
    public class SportEventInstanceDeleted : DomainEvent
    {
        public SportEventInstance DeletedSportEventInstance { get; set; }

        public SportEventInstanceDeleted(SportEventInstance deletedSportEventInstance)
        {
            DeletedSportEventInstance = deletedSportEventInstance;
        }
    }
}
