namespace PersonalNotesApi.Models;

// 1. 先定義一個 Enum 嚟限制分類嘅選項
public enum NoteCategory
{
    一般,
    日記,
    密碼
}
public class Note
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = "一般";  // 備忘錄、日記、密碼
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}