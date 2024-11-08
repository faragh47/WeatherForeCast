using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForeCastService _weatherForeCastService;

    public WeatherForecastController(IWeatherForeCastService weatherForeCastService)
    {
        _weatherForeCastService = weatherForeCastService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<string> Get() => await _weatherForeCastService.GetFromAPI();
}