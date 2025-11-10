// using NotesAPI.DTOs.Request;
using NotesAPI.DTOs.Request;
using NotesAPI.DTOs.Response;

namespace NotesAPI.Services.Interfaces;

public interface INoteService
{
    Task<IEnumerable<NoteDto>> GetAllNotesAsync(NoteFilterDto dto);
    Task<NoteDto?> GetNoteByIdAsync(Guid id);
    Task<NoteDto> CreateNoteAsync(CreateNoteDto dto);
    Task<NoteDto?> UpdateNoteAsync(Guid id, UpdateNoteDto dto);
    Task<NoteDto?> DeleteNoteAsync(Guid id);
}