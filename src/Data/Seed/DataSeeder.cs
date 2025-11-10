using Microsoft.EntityFrameworkCore;
using NotesAPI.Domain.Entities;

namespace NotesAPI.Data.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
    {
        try
        {
            // Seed in order of dependencies
            await SeedUsersAsync(context, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during data seeding");
            throw;
        }
    }

    private static async Task SeedUsersAsync(ApplicationDbContext context, ILogger logger)
    {
        if (await context.Users.AnyAsync())
        {
            logger.LogInformation("Users already exist, skipping seed");
            return;
        }

        logger.LogInformation("Seeding users...");

        var users = new List<UserEntity>
        {
            new UserEntity
            {
                Name = "admin",
                Email = "admin@gmail.com",
                Phone = "010959402",
                Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            },
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        logger.LogInformation("Users seeded successfully ({Count} users)", users.Count);
    }
}