using Microsoft.AspNetCore.Mvc;
using PersonalNotesApi.Models;
using PersonalNotesApi.Services;
//using PersonalNotesApi.Data;

namespace PersonalNotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;  // 改用 Service
    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }

    // GET: /api/notes
    [HttpGet]
    public IActionResult GetAll()
    {
        var notes = _noteService.GetAllNotes();
        return Ok(notes);
    }

    // GET: /api/notes/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var note = _noteService.GetNoteById(id);
        if (note == null)
        {
            return NotFound();
        }
        return Ok(note);
    }

    [HttpGet("category/{category}")] // 新的路由，例如: GET /api/notes/category/日記
public IActionResult GetByCategory(NoteCategory category)
{
    var notes = _noteService.GetNotesByCategory(category);
    return Ok(notes);
}

    // POST: /api/notes
    [HttpPost]
    public IActionResult Create([FromBody] Note newNote)
    {
        if (newNote == null)
        {
            return BadRequest();
        }

        newNote.CreatedAt = DateTime.Now;
        _noteService.CreateNote(newNote);

        return CreatedAtAction(nameof(GetById), new { id = newNote.Id }, newNote);
    }
}