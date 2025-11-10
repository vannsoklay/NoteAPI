using NotesAPI.Domain.Entities;

namespace NotesAPI.DTOs.Response;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    // Navigation property
    public UserEntity User { get; set; } = null!;
}