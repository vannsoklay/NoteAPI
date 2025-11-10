using System.Data;
using MySqlConnector;
using NotesAPI.Extensions;
using NotesAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("MySqlConnection");

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add custom configurations
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddCorsConfiguration();

// Dapper connection
builder.Services.AddScoped<IDbConnection>(_ => new MySqlConnection(connStr));

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Apply migrations and seed data
await app.ApplyMigrationsAsync();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Note API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<AuthMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Logger.LogInformation("🚀 Application started at {Time}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

app.Run();