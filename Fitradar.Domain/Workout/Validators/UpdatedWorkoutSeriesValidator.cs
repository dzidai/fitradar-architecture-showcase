using Fitradar.Domain.Common.Specifications;
using Fitradar.Domain.Common.Validation;
using Fitradar.Domain.Workout.Validators.Context;

namespace Fitradar.Domain.Workout.Validators
{
    public class UpdatedWorkoutSeriesValidator : NewWorkoutSeriesValidator
    {
        public UpdatedWorkoutSeriesValidator()
        {
            AddValidation("NoBookedTicketsValidation",

                new ValidationRule<WorkoutSeriesValidationContext>(
                    rule: new Specification<WorkoutSeriesValidationContext>(workoutSeries =>
                        workoutSeries.NumberOfBookedTickets == 0),
                    code: ValidationErrorCodes.UPDATE_ONLY_BEFORE_BOOKED_SEATS,
                    property: "NumberOfBookedTickets",
                    isWarning: false));

            AddValidation("NotCancelledValidation",

                new ValidationRule<WorkoutSeriesValidationContext>(
                    rule: new Specification<WorkoutSeriesValidationContext>(workoutSeries =>
                        !workoutSeries.IsCancelled),
                    code: ValidationErrorCodes.UPDATE_ONLY_NOT_CANCELLED,
                    property: "IsCancelled",
                    isWarning: false));
        }
    }
}
