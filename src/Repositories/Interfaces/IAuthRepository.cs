
using NotesAPI.Domain.Entities;

namespace NotesAPI.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<UserEntity?> LoginAsync(Guid id);
    Task<UserEntity> RegisterAsync(UserEntity user);
    Task<UserEntity?> GetProfileAsync(Guid userId);
    Task<UserEntity?> UserByEmailAsync(string email);
    Task<bool> NameExistsAsync(string name);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> PhoneExistsAsync(string phone);
}