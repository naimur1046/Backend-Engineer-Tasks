using Controllers.Models.Request;
using Controllers.Models.Response;
using Controllers.Services;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DateController : ControllerBase
{
    private readonly ILogger<DateController> _logger;
    private readonly IDateService _dateService;


    public DateController(ILogger<DateController> logger, IDateService dateService)
    {
        _logger = logger;
        _dateService = dateService;
    }
    
    [HttpPost("number-of-days")]
    public IActionResult GetNumberOfDays([FromBody] DateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        _logger.LogInformation("Calculating days between {StartDate} and {EndDate}",
            request.StartDate, request.EndDate);
        
        var days = _dateService.CalculateDaysBetween(request.StartDate, request.EndDate);
        
        return Ok(new DateResponse { Days = days });
    }
}