using System.ComponentModel.DataAnnotations;

namespace NotesAPI.DTOs.Request;

public class CreateNoteDto
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; } = string.Empty;
}