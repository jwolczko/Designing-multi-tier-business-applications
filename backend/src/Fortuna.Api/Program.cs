using Fortuna.Api.Middleware;
using Fortuna.Application.DependencyInjection;
using Fortuna.Infrastructure.DependencyInjection;
using Fortuna.ReadModel.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddReadModel(builder.Configuration);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapOpenApi("/openapi/v1.json");
app.MapControllers();
app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

app.Run();

public partial class Program
{
}
