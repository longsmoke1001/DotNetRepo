using System.ComponentModel;
using PersonalNotesApi.DTOs;
using PersonalNotesApi.Models;

namespace PersonalNotesApi.Services;

// INoteService.cs
public interface INoteService
{
    [Obsolete("Use GetNotesPagedAsync instead.")]
    Task<List<NoteDto>> GetAllNotesAsync(int userId);        // 改
    Task<NoteDto?> GetNoteByIdAsync(int id, int userId);       // 改
    Task<NoteDto> CreateNoteAsync(NoteDto dto, int userId);  // 改（回傳 NoteDto）
    Task<NoteDto?> UpdateNoteAsync(int id, NoteDto dto, int userId);  // 改
    Task<bool> DeleteNoteAsync(int id, int userId);            // 唔使改（Delete 冇回傳 Note）
    [Obsolete("Use GetNotesByCategoryPagedAsync instead.")]
    Task<List<NoteDto>> GetNotesByCategoryAsync(string category, int userId);  // 改
    Task<List<Note>> SearchNotesAsync(string query, int userId);
    Task<PaginationDto<NoteDto>> GetNotesPagedAsync(int pageNumber, int pageSize, int userId);
    Task<PaginationDto<NoteDto>> GetNotesByCategoryPagedAsync(string category, int pageNumber, int pageSize, int userId);
    Task<bool> DeleteAsync(int id, int userId);  // 新增 DeleteAsync 方法
    Task<bool> UpdateAsync(int id, NoteDto dto, int userId);  // 新增 UpdateAsync 方法
}