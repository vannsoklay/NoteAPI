
using NotesAPI.Domain.Entities;
using NotesAPI.DTOs.Request;

namespace NotesAPI.Repositories.Interfaces;

public interface INoteRepository
{
    Task<IEnumerable<NoteEntity>> GetAllAsync();
    Task<IEnumerable<NoteEntity>> QueryNotesAsync(NoteFilterDto dto);
    Task<NoteEntity?> GetByIdAsync(Guid id);
    Task<NoteEntity> CreateAsync(NoteEntity note);
    Task<NoteEntity?> UpdateAsync(NoteEntity note);
    Task<NoteEntity?> DeleteAsync(Guid id);
}