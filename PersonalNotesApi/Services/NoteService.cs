using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PersonalNotesApi.Controllers;
using PersonalNotesApi.DTOs;
using PersonalNotesApi.Models;

namespace PersonalNotesApi.Services;

public class NoteService : INoteService
{
    private readonly AppDbContext _context;

    public NoteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<NoteDto>> GetAllNotesAsync()
    {
        var notes = await _context.Notes
            .Select(n => new NoteDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                Category = n.Category.ToString(),
                CreatedAt = n.CreatedAt,
                UpdatedAt = n.UpdatedAt
            })
            .ToListAsync<NoteDto>();  // ✅ 明確指定類型

        return notes;
    }
    public async Task<NoteDto?> GetNoteByIdAsync(int id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null) return null;

        return new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            Category = note.Category.ToString(),
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }

    public async Task<NoteDto> CreateNoteAsync(NoteDto dto)
    {
        var note = new Note
        {
            Title = dto.Title,
            Content = dto.Content,
            Category = Enum.TryParse<NoteCategory>(dto.Category, out var category) ? category : NoteCategory.一般,
            CreatedAt = DateTime.Now
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
        return new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            Category = note.Category.ToString(),
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }

    public async Task<NoteDto?> UpdateNoteAsync(int id, NoteDto dto)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null) return null;

        note.Title = dto.Title;
        note.Content = dto.Content;
        note.Category = Enum.TryParse<NoteCategory>(dto.Category, out var category) ? category : NoteCategory.一般;
        note.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            Category = note.Category.ToString(),
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }

    public async Task<bool> DeleteNoteAsync(int id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null) return false;

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<NoteDto>?> GetNotesByCategoryAsync(string category)
    {
        // 將 string 轉成 Enum（如果轉換失敗，回傳空 List）
        if (!Enum.TryParse<NoteCategory>(category, true, out var categoryEnum))
        {
            return new List<NoteDto>();  // 或者 throw Exception
        }
        return await _context.Notes.Where(n => n.Category == categoryEnum)
                                        .Select(n => new NoteDto
                                        {
                                            Id = n.Id,
                                            Title = n.Title,
                                            Content = n.Content,
                                            Category = n.Category.ToString(),
                                            CreatedAt = n.CreatedAt,
                                            UpdatedAt = n.UpdatedAt
                                        }
        ).ToListAsync();
    }

    public List<Note> SearchNotes(string query)
    {
        return _context.Notes
                       .Where(n => n.Title.Contains(query) || n.Content.Contains(query))
                       .ToList();
    }
}