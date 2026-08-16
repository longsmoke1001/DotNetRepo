using Microsoft.AspNetCore.Mvc;
using PersonalNotesApi.Models;
using PersonalNotesApi.Services;
//using PersonalNotesApi.Data;

namespace PersonalNotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly AppDbContext _context;

    public NotesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /api/notes
    [HttpGet]
    public IActionResult GetAll()
    {
        GetAllNotes();
        return Ok(notes);
    }

    // GET: /api/notes/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var note = _context.Notes.Find(id);
        if (note == null)
        {
            return NotFound();
        }
        return Ok(note);
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
        _context.Notes.Add(newNote);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = newNote.Id }, newNote);
    }
}