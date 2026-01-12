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
    
    [HttpPost("temperature-stats-for-dhaka")]
    public async Task<IActionResult> GetTemperatureStatsForDhaka([FromBody] TemperatureRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _logger.LogInformation("Retrieving temperature details between {StartDate} and {EndDate}",
                request.StartDate, request.EndDate);

            var temperatureStates = await _temperatureService.GetTemperatureStatsAsync(request.StartDate, request.EndDate);

            return Ok(temperatureStates);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument provided for temperature stats request");
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "No temperature data available for the specified date range");
            return NotFound(new { error = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch temperature data from external API");
            return StatusCode(503, new { error = "Weather service temporarily unavailable. Please try again later." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while retrieving temperature stats");
            return StatusCode(500, new { error = "An unexpected error occurred. Please try again later." });
        }
    }
}