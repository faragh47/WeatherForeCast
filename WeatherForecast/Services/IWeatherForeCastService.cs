namespace WebApplication2.Services;

public interface IWeatherForeCastService
{
   public Task<string> GetFromAPI();
}