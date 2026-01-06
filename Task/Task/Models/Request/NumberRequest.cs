using System.ComponentModel.DataAnnotations;

namespace Controllers.Models.Request;

public class NumberRequest
{
    [Required]
    public int Number { get; set; }
}