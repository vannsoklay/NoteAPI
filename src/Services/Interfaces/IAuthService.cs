using NotesAPI.DTOs.Request;
using NotesAPI.DTOs.Response;

namespace NotesAPI.Services.Interfaces;

public interface IAuthService
{
    Task<UserDto> LoginUserAsync(LoginUserDto dto);
    Task<UserDto> RegisterUserAsync(RegisterUserDto dto);
    Task<UserDto?> GetProfileAsync(Guid id);
}