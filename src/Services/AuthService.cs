using NotesAPI.Domain.Entities;
using NotesAPI.DTOs.Request;
using NotesAPI.DTOs.Response;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<UserDto> LoginUserAsync(LoginUserDto dto)
    {

        var user = await _authRepository.UserByEmailAsync(dto.Email);

        if (user == null)
        {
            throw new Exception("Invalid email or password");
        }
        ;

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!isPasswordValid)
        {
            throw new Exception("Invalid email or password");
        }

        var auth = await _authRepository.LoginAsync(user.Id);
        if (auth == null)
        {
            throw new Exception("Invalid email or password");
        }

        return MapToResponse(auth);
    }

    public async Task<UserDto> RegisterUserAsync(RegisterUserDto dto)
    {
        // Validate: Check if email exists
        if (await _authRepository.EmailExistsAsync(dto.Email))
            throw new Exception($"Email '{dto.Email}' is already registered");

        // Validate: Check if name exists
        if (await _authRepository.NameExistsAsync(dto.Name))
            throw new Exception($"Name '{dto.Name}' is already taken");

        // Validate: Check if phone exists
        // Validate phone only if provided
        if (!string.IsNullOrWhiteSpace(dto.Phone) && await _authRepository.PhoneExistsAsync(dto.Phone))
            throw new Exception($"Phone number '{dto.Phone}' is already taken");

        var user = new UserEntity
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow
        };
        var newUser = await _authRepository.RegisterAsync(user);
        return MapToResponse(newUser);
    }

    public async Task<UserDto?> GetProfileAsync(Guid userId)
    {
        var user = await _authRepository.GetProfileAsync(userId);
        if (user == null) return null;

        return MapToResponse(user);
    }

    private static UserDto MapToResponse(UserEntity user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };
    }
}