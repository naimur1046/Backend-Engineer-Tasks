using System.ComponentModel.DataAnnotations;
using Controllers.Services;

namespace Controllers.Models.Request;

public class TemperatureRequest : IValidatableObject
{
    [Required(ErrorMessage = "StartDate is required")]
    public string StartDate { get; set; } = string.Empty;

    [Required(ErrorMessage = "EndDate is required")]
    public string EndDate { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var dateService = validationContext.GetService(typeof(IDateService)) as IDateService;

        if (dateService == null)
        {
            dateService = new DateService();
        }

        var isStartDateValid = dateService.IsValidDate(StartDate);
        var isEndDateValid = dateService.IsValidDate(EndDate);

        if (!isStartDateValid)
        {
            yield return new ValidationResult(
                "StartDate must be in format YYYY-MM-DD with valid date values",
                new[] { nameof(StartDate) });
        }

        if (!isEndDateValid)
        {
            yield return new ValidationResult(
                "EndDate must be in format YYYY-MM-DD with valid date values",
                new[] { nameof(EndDate) });
        }

        // Only validate date order if both dates are valid
        if (isStartDateValid && isEndDateValid && !dateService.IsStartDateBeforeOrEqualEndDate(StartDate, EndDate))
        {
            yield return new ValidationResult(
                "StartDate must be before or equal to EndDate",
                new[] { nameof(StartDate), nameof(EndDate) });
        }
    }
}