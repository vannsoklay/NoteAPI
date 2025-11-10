using NotesAPI.Domain.Entities;

namespace NotesAPI.DTOs.Response;

public class NoteDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    // Navigation property
    public UserEntity User { get; set; } = null!;
}