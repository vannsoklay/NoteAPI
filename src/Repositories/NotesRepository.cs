using System.Data;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Domain.Entities;
using NotesAPI.Repositories.Interfaces;
using Dapper;
using NotesAPI.DTOs.Request;

namespace NotesAPI.Repositories;

public class NotesRepository : INoteRepository
{
    private readonly IDbConnection _db;

    public NotesRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<NoteEntity>> GetAllAsync()
    {
        var sql = @"SELECT id, title, content, user_id AS UserId, created_at AS CreatedAt, updated_at AS UpdatedAt, deleted_at AS DeletedAt
                    FROM notes
                    WHERE deleted_at IS NULL
                    ORDER BY created_at DESC";

        return await _db.QueryAsync<NoteEntity>(sql);
    }

    public async Task<IEnumerable<NoteEntity>> QueryNotesAsync(NoteFilterDto dto)
    {
        var sql = @"
        SELECT id, title, content, user_id AS UserId, created_at AS CreatedAt, 
               updated_at AS UpdatedAt, deleted_at AS DeletedAt
        FROM notes
        WHERE deleted_at IS NULL
        ";

        // Searching
        if (!string.IsNullOrEmpty(dto.Search))
        {
            sql += " AND (LOWER(title) LIKE @Search OR LOWER(content) LIKE @Search)";
        }

        // Sorting
        sql += dto.SortBy switch
        {
            "oldest" => " ORDER BY created_at ASC",
            "title_asc" => " ORDER BY title ASC",
            "title_desc" => " ORDER BY title DESC",
            _ => " ORDER BY created_at DESC" // default newest first
        };

        return await _db.QueryAsync<NoteEntity>(sql, new
        {
            Search = $"%{dto.Search?.ToLower()}%"
        });
    }


    public async Task<NoteEntity?> GetByIdAsync(Guid id)
    {
        return await _db.QueryFirstOrDefaultAsync<NoteEntity>("SELECT * FROM notes WHERE id = @Id AND deleted_at IS NULL", new { Id = id });
    }


    public async Task<NoteEntity> CreateAsync(NoteEntity note)
    {
        note.Id = Guid.NewGuid();
        note.CreatedAt = DateTime.UtcNow;
        note.UpdatedAt = DateTime.UtcNow;

        const string sql = @"
            INSERT INTO notes (id, title, content, user_id, created_at, updated_at)
            VALUES (@Id, @Title, @Content, @UserId, @CreatedAt, @UpdatedAt);
        ";

        await _db.ExecuteAsync(sql, note);
        return note;
    }

    public
    async Task<NoteEntity?> UpdateAsync(NoteEntity note)
    {
        note.UpdatedAt = DateTime.UtcNow;

        const string sql = @"
            UPDATE notes
            SET title = @Title,
                content = @Content,
                updated_at = @UpdatedAt
            WHERE id = @Id AND deleted_at IS NULL;
        ";

        var rows = await _db.ExecuteAsync(sql, note);
        return rows > 0 ? note : null;
    }

    public async Task<NoteEntity?> DeleteAsync(Guid id)
    {
        var note = await GetByIdAsync(id);
        if (note == null) return null;

        const string sql = @"UPDATE notes SET deleted_at = NOW() WHERE id = @Id;";

        await _db.ExecuteAsync(sql, new { Id = id });
        return note;
    }
}