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
            Category = Enum.TryParse<NoteCategory>(dto.Category, out var category) ? category : NoteCategory.general,
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
        note.Category = Enum.TryParse<NoteCategory>(dto.Category, out var category) ? category : NoteCategory.general;
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

    public async Task<List<NoteDto>> GetNotesByCategoryAsync(string category)
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

    public async Task<PaginationDto<NoteDto>> GetNotesPagedAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Notes.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var notes = await _context.Notes
            .OrderBy(n => n.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NoteDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                Category = n.Category.ToString(),
                CreatedAt = n.CreatedAt,
                UpdatedAt = n.UpdatedAt
            })
            .ToListAsync();

        return new PaginationDto<NoteDto>
        {
            Items = notes,
            TotalCount = totalCount,
            TotalPages = totalPages,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<PaginationDto<NoteDto>> GetNotesByCategoryPagedAsync(string category, int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        if (!Enum.TryParse<NoteCategory>(category, true, out var categoryEnum))
        {
            return new PaginationDto<NoteDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 0,
                TotalPages = 0,
                Items = new List<NoteDto>()
            };
        }

        var query = _context.Notes.Where(n => n.Category == categoryEnum);
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var notes = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NoteDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                Category = n.Category.ToString(),
                CreatedAt = n.CreatedAt,
                UpdatedAt = n.UpdatedAt
            })
            .ToListAsync();

        return new PaginationDto<NoteDto>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Items = notes
        };
    }
    public async Task<List<Note>> SearchNotesAsync(string query)
    {
        return await _context.Notes
                       .Where(n => n.Title.Contains(query) || n.Content.Contains(query))
                       .ToListAsync();
    }

}