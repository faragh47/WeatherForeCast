using WebApplication2;
using WebApplication2.Data;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
AppDbContextInitialiser.AppDatabase(builder.Services,builder.Configuration);
builder.Services.AddScoped<IWeatherForeCastService, WeatherForeCastService>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.InitialiseAsync();
app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }
    await next();
});
app.MapControllers();

app.Run();


