using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotesAPI.Data;
using NotesAPI.Repositories;
using NotesAPI.Services;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<INoteRepository, NotesRepository>();

        // Register services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<INoteService, NotesService>();

        return services;
    }

    public static IServiceCollection AddDatabaseConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MySqlConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                })
        );

        return services;
    }

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Note API",
                Description = "ASP.NET Core 9 Web API with MySQL Database",
                Contact = new OpenApiContact
                {
                    Name = "vannsoklay",
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
        });

        return services;
    }
}