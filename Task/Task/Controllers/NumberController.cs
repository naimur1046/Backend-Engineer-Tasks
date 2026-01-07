using Controllers.Models.Request;
using Controllers.Models.Response;
using Controllers.Services;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NumberController : ControllerBase
{
    private readonly ILogger<NumberController> _logger;
    private readonly INumberService _numberService;

    public NumberController(ILogger<NumberController> logger, INumberService numberService)
    {
        _logger = logger;
        _numberService = numberService;
    }

    [HttpPost("number-to-words")]
    public IActionResult GetNumberToWords([FromBody] NumberRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Converting {number} to words", request.Number);

        var words = _numberService.ConvertToWords(request.Number);

        return Ok(new NumberResponse { Words = words });
    }
}