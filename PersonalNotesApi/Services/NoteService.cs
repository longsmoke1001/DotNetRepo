using PersonalNotesApi.Models;

namespace PersonalNotesApi.Services;

public class NoteService : INoteService
{
    private readonly AppDbContext _context;

    public NoteService(AppDbContext context)
    {
        _context = context;
    }

    public List<Note> GetAllNotes()
    {
        return _context.Notes.ToList();
    }

    public Note? GetNoteById(int id)
    {
        return _context.Notes.Find(id);
    }

    public Note CreateNote(Note note)
    {
        _context.Notes.Add(note);
        _context.SaveChanges();
        return note;
    }

    public Note? UpdateNote(int id, Note updatedNote)
    {
        var note = _context.Notes.Find(id);
        if (note == null) return null;

        note.Title = updatedNote.Title;
        note.Content = updatedNote.Content;

        _context.SaveChanges();
        return note;
    }

    public bool DeleteNote(int id)
    {
        var note = _context.Notes.Find(id);
        if (note == null) return false;

        _context.Notes.Remove(note);
        _context.SaveChanges();
        return true;
    }

    public List<Note> GetNotesByCategory(NoteCategory category)
    {
        return _context.Notes
                       .Where(n => n.Category == category)
                       .ToList();
    }
}