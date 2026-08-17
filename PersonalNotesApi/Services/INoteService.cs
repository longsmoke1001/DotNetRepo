using System.ComponentModel;
using PersonalNotesApi.DTOs;
using PersonalNotesApi.Models;

namespace PersonalNotesApi.Services;

// INoteService.cs
public interface INoteService
{
    [Obsolete("Use GetNotesPagedAsync instead.")]
    Task<List<NoteDto>> GetAllNotesAsync();        // 改
    Task<NoteDto?> GetNoteByIdAsync(int id);       // 改
    Task<NoteDto> CreateNoteAsync(NoteDto dto);  // 改（回傳 NoteDto）
    Task<NoteDto?> UpdateNoteAsync(int id, NoteDto dto);  // 改
    Task<bool> DeleteNoteAsync(int id);            // 唔使改（Delete 冇回傳 Note）
    [Obsolete("Use GetNotesByCategoryPagedAsync instead.")]
    Task<List<NoteDto>> GetNotesByCategoryAsync(string category);
    Task<List<Note>> SearchNotesAsync(string query);
    Task<PaginationDto<NoteDto>> GetNotesPagedAsync(int pageNumber, int pageSize);
    Task<PaginationDto<NoteDto>> GetNotesByCategoryPagedAsync(string category, int pageNumber, int pageSize);
}