namespace NotesAPI.DTOs.Request;

public class UpdateNoteDto
{
    public string? Title { get; set; } = string.Empty;
    public string? Content { get; set; } = string.Empty;
}