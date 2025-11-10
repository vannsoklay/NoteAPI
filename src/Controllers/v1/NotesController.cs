using Microsoft.AspNetCore.Mvc;
using NotesAPI.DTOs.Request;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Controllers.v1;

[ApiController]
[Route("api/v1/notes")]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;
    private readonly ILogger<NotesController> _logger;

    public NotesController(INoteService noteService, ILogger<NotesController> logger)
    {
        _noteService = noteService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllNotes([FromQuery] NoteFilterDto dto)
    {
        var notes = await _noteService.GetAllNotesAsync(dto);
        return Ok(new { success = true, data = notes });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetNoteById(Guid id)
    {
        var note = await _noteService.GetNoteByIdAsync(id);
        return Ok(new { success = true, data = note });
    }

    [HttpPost]
    public async Task<ActionResult> CreateNoteAsync(CreateNoteDto dto)
    {
        var newNote = await _noteService.CreateNoteAsync(dto);
        return Ok(new { success = true, data = newNote });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateNoteAsync(Guid id, UpdateNoteDto dto)
    {
        var updateNote = await _noteService.UpdateNoteAsync(id, dto);
        return Ok(new { success = true, data = updateNote });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteNoteAsync(Guid id)
    {
        var deletedNote = await _noteService.DeleteNoteAsync(id);
        return Ok(new { success = true, data = deletedNote });
    }
}