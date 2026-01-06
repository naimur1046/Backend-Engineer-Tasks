using Controllers.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NumberController : ControllerBase
{
    private readonly ILogger<NumberController> _logger;
    
    public NumberController(ILogger<NumberController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost("number-to-words")]
    public IActionResult GetNumberToWords([FromBody] NumberRequest request)
    {
        _logger.LogInformation("Converting {number} to words", 
            request.Number);
        
        return Ok();
    }
}