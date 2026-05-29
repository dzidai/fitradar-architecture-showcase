using Fitradar.Domain.Common.Specifications;
using Fitradar.Domain.Common.Validation;
using Fitradar.Domain.Workout.Validators.Context;
using System;
using System.Linq;

namespace Fitradar.Domain.Workout.Validators
{
    public class NewSportEventValidator : EntityValidatorBase<SportEventValidationContext>
    {
        public NewSportEventValidator()
        {
            AddValidation("MinStartTimeValidation",

               new ValidationRule<SportEventValidationContext>(
                   rule: new Specification<SportEventValidationContext>(sportEvent =>
                       sportEvent.TimeSlots.All(timeSlot =>
                            timeSlot.StartTime >= DateTime.UtcNow.AddMinutes(BusinessRulesConstants.MIN_NUMBER_OF_MINUTES_BEFORE_EVENT_STARTS)
                       )
                   ),
                   code: ValidationErrorCodes.EVENT_START_TIME_TOO_EARLY,
                   property: "start_time",
                   isWarning: false));

            AddValidation("MaxStartTimeValidation",

               new ValidationRule<SportEventValidationContext>(
                   rule: new Specification<SportEventValidationContext>(sportEvent =>
                       sportEvent.IsFreeOfCharge || sportEvent.TimeSlots.All(timeSlot =>
                            timeSlot.StartTime <= DateTime.UtcNow.AddDays(BusinessRulesConstants.MAX_DAYS_UNTIL_EVENT_STARTS)
                       )
                   ),
                   code: ValidationErrorCodes.EVENT_START_DATE_TOO_BIG,
                   property: "start_time",
                   isWarning: false));

            AddValidation("MaxEndTimeValidation",

               new ValidationRule<SportEventValidationContext>(
                   rule: new Specification<SportEventValidationContext>(sportEvent =>
                       sportEvent.TimeSlots.All(timeSlot =>
                            (timeSlot.EndTime - timeSlot.StartTime).TotalHours <= BusinessRulesConstants.MAX_HOURS_OF_EVENT_DURATION
                       )
                   ),
                   code: ValidationErrorCodes.EVENT_DURATION_TOO_BIG,
                   property: "end_time",
                   isWarning: false));

            AddValidation("MinPriceValidation",

               new ValidationRule<SportEventValidationContext>(
                   rule: new Specification<SportEventValidationContext>(sportEvent =>
                        sportEvent.IsFreeOfCharge ||
                        sportEvent.Price != null && sportEvent.Price.Amount >= BusinessRulesConstants.STRIPE_MIN_CHARGE_TABLE[sportEvent.Price.Currency.ToUpper()] * 2),
                   code: ValidationErrorCodes.EVENT_PRICE_TOO_SMALL,
                   property: "price",
                   isWarning: false));
        }
    }
}
