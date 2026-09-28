using Microsoft.EntityFrameworkCore;
using PersonalNotesApi.DTOs;
using PersonalNotesApi.Models;
using PersonalNotesApi.Data;
namespace PersonalNotesApi.Services;

public class NoteService : INoteService
{
    private readonly AppDbContext _context;

    public NoteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<NoteDto>> GetAllNotesAsync(int userId)
    {
        var notes = await _context.Notes
            .Where(n => n.UserId == userId)  // 只取屬於該用戶嘅筆記
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

        return notes;
    }

    public async Task<NoteDto?> GetNoteByIdAsync(int id, int userId)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
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

    public async Task<NoteDto> CreateNoteAsync(NoteDto dto, int userId)
    {
        var note = new Note
        {
            UserId = userId,  // 設定 UserId
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

    public async Task<NoteDto?> UpdateNoteAsync(int id, NoteDto dto, int userId)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
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

    public async Task<bool> DeleteNoteAsync(int id, int userId)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (note == null) return false;

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<NoteDto>> GetNotesByCategoryAsync(string category, int userId)
    {
        // 將 string 轉成 Enum（如果轉換失敗，回傳空 List）
        if (!Enum.TryParse<NoteCategory>(category, true, out var categoryEnum))
        {
            return new List<NoteDto>();  // 或者 throw Exception
        }
        return await _context.Notes
            .Where(n => n.UserId == userId && n.Category == categoryEnum)
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
    }

    public async Task<PaginationDto<NoteDto>> GetNotesPagedAsync(int pageNumber, int pageSize, int userId)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _context.Notes.Where(n => n.UserId == userId);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var notes = await query
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

    public async Task<PaginationDto<NoteDto>> GetNotesByCategoryPagedAsync(string category, int pageNumber, int pageSize, int userId)
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

        var query = _context.Notes
            .Where(n => n.UserId == userId && n.Category == categoryEnum);
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

    public async Task<List<Note>> SearchNotesAsync(string query, int userId)
    {
        return await _context.Notes
            .Where(n => n.UserId == userId &&
                        (n.Title.Contains(query) || n.Content.Contains(query)))
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (note == null) return false;

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, NoteDto dto, int userId)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (note == null) return false;

        note.Title = dto.Title;
        note.Content = dto.Content;
        note.Category = Enum.TryParse<NoteCategory>(dto.Category, out var category) ? category : NoteCategory.general;
        note.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }
}
