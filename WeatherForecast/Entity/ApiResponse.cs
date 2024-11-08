namespace WebApplication2.Controllers;

public class ApiResponse
{
    public int Id { get; set; }
    public DateTime FetchedAt { get; set; }
    public string RawResponse { get; set; }
}