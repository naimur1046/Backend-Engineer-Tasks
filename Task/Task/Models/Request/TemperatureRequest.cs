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

        if (!dateService.IsValidDate(StartDate))
        {
            yield return new ValidationResult(
                "StartDate must be in format YYYY-MM-DD with valid date values",
                new[] { nameof(StartDate) });
        }

        if (!dateService.IsValidDate(EndDate))
        {
            yield return new ValidationResult(
                "EndDate must be in format YYYY-MM-DD with valid date values",
                new[] { nameof(EndDate) });
        }
    }
}