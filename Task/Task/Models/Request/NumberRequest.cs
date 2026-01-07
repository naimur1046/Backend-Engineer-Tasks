using System.ComponentModel.DataAnnotations;

namespace Controllers.Models.Request;

public class NumberRequest : IValidatableObject
{
    [Required]
    [Range(0, 999.99, ErrorMessage = "Number must be between 0 and 999.99")]
    public decimal Number { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(Number)[3])[2];
        if (decimalPlaces > 2)
        {
            yield return new ValidationResult(
                "Number can have at most 2 decimal places",
                new[] { nameof(Number) });
        }
    }
}