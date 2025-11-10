using NotesAPI.Domain.Entities;
using NotesAPI.DTOs.Request;
using NotesAPI.DTOs.Response;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Services;

public class NotesService : INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly ILogger<NotesService> _logger;

    public NotesService(INoteRepository noteRepository, ILogger<NotesService> logger)
    {
        _noteRepository = noteRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<NoteDto>> GetAllNotesAsync(NoteFilterDto dto)
    {
        var useFilter =
       !string.IsNullOrWhiteSpace(dto.Search) ||
       !string.IsNullOrWhiteSpace(dto.SortBy);

        IEnumerable<NoteEntity> notes;

        if (useFilter)
        {
            notes = await _noteRepository.QueryNotesAsync(dto);
        }
        else
        {
            notes = await _noteRepository.GetAllAsync();
        }

        return notes.Select(MapToResponse);
    }

    public async Task<NoteDto?> GetNoteByIdAsync(Guid id)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        return note != null ? MapToResponse(note) : null;
    }

    public async Task<NoteDto> CreateNoteAsync(CreateNoteDto dto)
    {
        var note = new NoteEntity
        {
            Title = dto.Title,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow,
            // UserId = Guid.Parse("08de1ef2-23e4-4fe1-814e-e3d6e47bb39f")
        };

        var newNote = await _noteRepository.CreateAsync(note);
        return MapToResponse(newNote);
    }

    public async Task<NoteDto?> UpdateNoteAsync(Guid id, UpdateNoteDto dto)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        if (note == null) return null;

        note.Title = dto.Title;
        note.Content = dto.Content;

        var updatedNote = await _noteRepository.UpdateAsync(note);
        return MapToResponse(updatedNote);
    }

    public async Task<NoteDto?> DeleteNoteAsync(Guid id)
    {
        var deletedNote = await _noteRepository.DeleteAsync(id);
        return MapToResponse(deletedNote);
    }

    private static NoteDto MapToResponse(NoteEntity note)
    {
        return new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt,
        };
    }
}