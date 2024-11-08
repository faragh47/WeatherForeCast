using Microsoft.EntityFrameworkCore;
using WebApplication2.APIClient;
using WebApplication2.Controllers;
using WebApplication2.Data;

namespace WebApplication2.Services;

public class WeatherForeCastService:IWeatherForeCastService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly APIClientFactory _apiClientFactory;
    private readonly ApiClientCoordinator _apiClientCoordinator;
    private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1,1);
    private const string APIURL = "https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&hourly=temperature_2m";
    public WeatherForeCastService(ApplicationDbContext applicationDbContext,
        IHttpClientFactory httpClientFactory,
        ILogger<WeatherForecastController> logger)
    {
        _applicationDbContext = applicationDbContext;
        _apiClientFactory = new(httpClientFactory);
        _apiClientCoordinator = new ApiClientCoordinator(this,logger);
    }
    public async Task<string> GetFromDb()
    {
        await _semaphoreSlim.WaitAsync(); 
        var result = (await _applicationDbContext.ApiResponses.FirstOrDefaultAsync())?.RawResponse;
        _semaphoreSlim.Release();
        return result;
    }
    public async Task<string> GetFromAPI() =>
        await _apiClientCoordinator.GetFromAPI(_apiClientFactory.GetFromApi, APIURL);
    public async Task<string> UpdateDBFromResponse(string response)
    {
        await _semaphoreSlim.WaitAsync();
        await updateDB(response);
        _semaphoreSlim.Release();
        return response;
    }

    private async Task updateDB(string response)
    {
        var exist = await _applicationDbContext.ApiResponses.FirstOrDefaultAsync();
        if (exist is null)
        {
            var apiResponse = new ApiResponse
            {
                FetchedAt = DateTime.UtcNow,
                RawResponse = response
            };
            _applicationDbContext.ApiResponses.Add(apiResponse);
        }
        else
        {
            exist.RawResponse = response;
            exist.FetchedAt = DateTime.UtcNow;
            _applicationDbContext.ApiResponses.Update(exist);
        }
        await _applicationDbContext.SaveChangesAsync();
    }
}