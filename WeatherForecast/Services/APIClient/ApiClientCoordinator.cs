using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using WebApplication2.Controllers;
using WebApplication2.Services;

namespace WebApplication2.APIClient;

public class ApiClientCoordinator
{
    private readonly WeatherForeCastService _weatherForeCastService;
    private readonly ILogger<WeatherForecastController> _logger;
    public delegate Task<string> ResponseDelegate(string url);
    private const int TIMEOUT = 5;
    private const int RETRYCOUNT = 3;
    private const int SLEEPDURATIONPROVIDER = 3;
    public ApiClientCoordinator(WeatherForeCastService weatherForeCastService,
                                ILogger<WeatherForecastController> logger)
    {
        _weatherForeCastService = weatherForeCastService;
        _logger = logger;
    }
    public async Task<string> GetFromAPI(ResponseDelegate responseDelegate, string url)
    {
        var retryPolicy = addRetryPolicy();
        var timeoutPolicy = addTimeOutPolicy();
        var policyWrap = Policy.WrapAsync(retryPolicy, timeoutPolicy);
        try
        {
            var result = await policyWrap.ExecuteAsync(async () =>
            {
                var response = await responseDelegate(url);
                return response;
            });
            if (!string.IsNullOrEmpty(result))
                await _weatherForeCastService.UpdateDBFromResponse(result);
            return result;
        }
        catch (TimeoutRejectedException exception)
        {
            _logger.Log(LogLevel.Warning,exception.Message);
            return await _weatherForeCastService.GetFromDb();
        }
        catch (Exception exception)
        {
            _logger.Log(LogLevel.Error,exception.Message);
            return await _weatherForeCastService.GetFromDb();
        }
    }

    private IAsyncPolicy addTimeOutPolicy() =>
        Policy
            .TimeoutAsync(TIMEOUT, TimeoutStrategy.Pessimistic);

    private IAsyncPolicy addRetryPolicy() => Policy
        .Handle<HttpRequestException>()
        .WaitAndRetryAsync(
            retryCount: RETRYCOUNT,
            sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(SLEEPDURATIONPROVIDER, attempt)),
            onRetry: (exception, duration, attempt, context) =>
            {
                _logger.Log(LogLevel.Warning,exception.Message);
            });
}