using System.ComponentModel.DataAnnotations;

namespace Controllers.Models.Response;

public class NumberResponse
{
    [Required]
    public string Words { get; set; }
}