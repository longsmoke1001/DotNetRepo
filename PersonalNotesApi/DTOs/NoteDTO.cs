namespace PersonalNotesApi.DTOs;
using PersonalNotesApi.Models;
// 1. 先定義一個 Enum 嚟限制分類嘅選項

public class NoteDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}