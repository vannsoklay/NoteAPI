using System.Data;
using Dapper;
using NotesAPI.Domain.Entities;
using NotesAPI.Repositories.Interfaces;

namespace NotesAPI.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly IDbConnection _db;

    public AuthRepository(IDbConnection db)
    {
        _db = db;
    }

    // Dapper: Login
    public async Task<UserEntity?> LoginAsync(Guid id)
    {
        string sql = "SELECT * FROM users WHERE id = @Id AND deleted_at IS NULL LIMIT 1";

        return await _db.QueryFirstOrDefaultAsync<UserEntity>(sql, new { Id = id });
    }

    // Dapper: Register
    public async Task<UserEntity> RegisterAsync(UserEntity user)
    {
        const string sql = @"
            INSERT INTO users (id, name, email, phone, password, created_at)
            VALUES (@Id, @Name, @Email, @Phone, @Password, @CreatedAt);
        ";

        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;

        await _db.ExecuteAsync(sql, user);

        return user;
    }

    // Dapper: Get profile
    public async Task<UserEntity?> GetProfileAsync(Guid userId)
    {
        const string sql = @"
            SELECT id, name, email, phone, created_at 
            FROM users 
            WHERE id = @UserId AND deleted_at IS NULL
            LIMIT 1;
        ";

        return await _db.QueryFirstOrDefaultAsync<UserEntity>(sql, new { UserId = userId });
    }

    // Dapper: Get user by email
    public async Task<UserEntity?> UserByEmailAsync(string email)
    {
        string sql = "SELECT * FROM users WHERE email = @Email AND deleted_at IS NULL";
        return await _db.QueryFirstOrDefaultAsync<UserEntity>(sql, new { Email = email });
    }

    // Dapper: Check email exists
    public async Task<bool> EmailExistsAsync(string email)
    {
        string sql = "SELECT 1 FROM users WHERE email = @Email LIMIT 1";
        var result = await _db.ExecuteScalarAsync<int?>(sql, new { Email = email });
        return result.HasValue;
    }

    // Dapper: Check name exists
    public async Task<bool> NameExistsAsync(string name)
    {
        string sql = "SELECT 1 FROM users WHERE name = @Name LIMIT 1";
        var result = await _db.ExecuteScalarAsync<int?>(sql, new { Name = name });
        return result.HasValue;
    }

    // Dapper: Check phone exists
    public async Task<bool> PhoneExistsAsync(string phone)
    {
        string sql = "SELECT 1 FROM users WHERE phone = @Phone LIMIT 1";
        var result = await _db.ExecuteScalarAsync<int?>(sql, new { Phone = phone });
        return result.HasValue;
    }
}