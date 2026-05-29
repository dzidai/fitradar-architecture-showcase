using Fitradar.Domain.Common;

namespace Fitradar.Domain.Workout.Events
{
    public class SportEventInstanceUpdated : DomainEvent
    {
        public SportEventInstance UpdatedSportEventInstance { get; set; }

        public SportEventInstanceUpdated(SportEventInstance updatedSportEventInstance)
        {
            UpdatedSportEventInstance = updatedSportEventInstance;
        }
    }
}
