using Fitradar.Domain.Common.Specifications;
using Fitradar.Domain.Common.Validation;
using Fitradar.Domain.Workout.Validators.Context;

namespace Fitradar.Domain.Workout.Validators
{
    public class UpdatedEventValidator : NewSportEventValidator
    {
        public UpdatedEventValidator()
        {
            AddValidation("BookedSeatsValidation",

                new ValidationRule<SportEventValidationContext>(
                    rule: new Specification<SportEventValidationContext>(sportEvent =>
                        sportEvent.NumberOfBookedTickets == 0),
                    code: ValidationErrorCodes.UPDATE_ONLY_BEFORE_BOOKED_SEATS,
                    property: "NumberOfBookedTickets",
                    isWarning: false));

            AddValidation("CancelledValidation",

                new ValidationRule<SportEventValidationContext>(
                    rule: new Specification<SportEventValidationContext>(sportEventInstance =>
                        sportEventInstance.IsCancelled == false),
                    code: ValidationErrorCodes.UPDATE_ONLY_NOT_CANCELLED,
                    property: "IsCancelled",
                    isWarning: false));
        }
    }
}
