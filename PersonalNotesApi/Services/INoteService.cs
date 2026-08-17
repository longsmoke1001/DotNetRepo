using PersonalNotesApi.Models;

namespace PersonalNotesApi.Services;

public interface INoteService
{
    List<Note> GetAllNotes();
    Note? GetNoteById(int id);
    Note CreateNote(Note note);
    Note? UpdateNote(int id, Note updatedNote);
    bool DeleteNote(int id);
    List<Note> GetNotesByCategory(NoteCategory category); // 新方法
    List<Note> SearchNotes(string query); // 新方法
}