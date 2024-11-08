using WebApplication2.Controllers;
namespace WebApplication2.APIClient;
public class APIClientFactory
{
    private IHttpClientFactory _httpClientFactory;
    private HttpClient _httpClient;
    public APIClientFactory(IHttpClientFactory _httpClientFactory)
    {   
         _httpClient = _httpClientFactory.CreateClient();
    }
    public async Task<string> GetFromApi(string url)=> await _httpClient?.GetStringAsync(url);
}