using Microsoft.AspNetCore.Mvc;

namespace Organizarty.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    public IActionResult Get()
    {
        return Ok("Tudo bom");
    }
}