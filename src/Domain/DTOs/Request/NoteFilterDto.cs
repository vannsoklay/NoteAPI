
namespace NotesAPI.DTOs.Request;

public class NoteFilterDto
{
    public string? Search { get; set; } = "";
    public string? SortBy { get; set; } = "newest"; // newest, oldest, title_asc, title_desc
}