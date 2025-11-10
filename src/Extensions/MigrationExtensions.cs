using Microsoft.EntityFrameworkCore;
using NotesAPI.Data;
using NotesAPI.Data.Seed;

namespace NotesAPI.Extensions;

public static class MigrationExtensions
{
    public static async Task<IApplicationBuilder> ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        var environment = services.GetRequiredService<IWebHostEnvironment>();

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            logger.LogInformation("Database Migration & Seeding" + "\n" +
                                  "Time: 2025-11-08 13:55:44 UTC" + "\n" +
                                  "Environment: {Environment}", environment.EnvironmentName);

            // Step 1: Test database connection
            logger.LogInformation("Step 1/3: Testing database connection...");

            if (!await context.Database.CanConnectAsync())
            {
                logger.LogError("Cannot connect to database. Please check:" + "\n" + "• Connection string in appsettings.json" + "\n" + "• Database credentials are correct");
                throw new Exception("Database connection failed");
            }

            logger.LogInformation("Database connection successful");

            // Step 2: Check and apply migrations
            logger.LogInformation("Step 2/3: Checking for pending migrations...");

            var pendingMigrations = (await context.Database.GetPendingMigrationsAsync()).ToList();

            if (pendingMigrations.Any())
            {
                logger.LogInformation("Applying pending migrations:");

                foreach (var migration in pendingMigrations)
                {
                    logger.LogInformation("{Migration}", migration);
                }

                logger.LogInformation("Applying migrations to database...");

                await context.Database.MigrateAsync();

                logger.LogInformation("All migrations applied successfully");

                // Log final migration state
                var finalAppliedMigrations = await context.Database.GetAppliedMigrationsAsync();
                logger.LogInformation(" • Total applied migrations: {Count}", finalAppliedMigrations.Count());
            }
            else
            {
                logger.LogInformation("Database is up to date (no pending migrations)");
            }

            // Step 3: Seed data
            logger.LogInformation("");
            logger.LogInformation("Step 3/3: Seeding initial data...");

            await DataSeeder.SeedAsync(context, logger);

            logger.LogInformation("Database initialization completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database initialization failed");

            // In development, we might want to continue; in production, we should stop
            if (!environment.IsDevelopment())
            {
                throw;
            }
        }

        return app;
    }
}