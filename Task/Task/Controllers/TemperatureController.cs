using Controllers.Models.Request;
using Controllers.Models.Response;
using Controllers.Services;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemperatureController : ControllerBase
{
    private readonly ILogger<TemperatureController> _logger;
    private readonly ITemperatureService _temperatureService;
    
    public TemperatureController(ILogger<TemperatureController> logger, ITemperatureService temperatureService)
    {
        _logger = logger;
        _temperatureService = temperatureService;
    }
    
    [HttpPost("temperature-stats-for-dhaka ")]
    public IActionResult GetTemperatureStatsForDhaka([FromBody] TemperatureRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Retrieving temperature details between {StartDate} and {EndDate}",
            request.StartDate, request.EndDate);

        var temperatureStates = _temperatureService.GetTemperatureStatsAsync(request.StartDate, request.EndDate);

        return Ok(new TemperatureResponse() );
    }
}