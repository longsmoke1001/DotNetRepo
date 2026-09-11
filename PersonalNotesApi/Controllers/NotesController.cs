using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalNotesApi.DTOs;
using PersonalNotesApi.Models;
using PersonalNotesApi.Services;
//using PersonalNotesApi.Data;

namespace PersonalNotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;  // 改用 Service
    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }

    // GET: /api/notes
    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
    {
        var notes = await _noteService.GetNotesPagedAsync(pageNumber, pageSize);
        return Ok(notes);
    }

    // GET: /api/notes/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var note = await _noteService.GetNoteByIdAsync(id);
        if (note == null)
        {
            return NotFound();
        }
        return Ok(note);
    }

    [HttpGet("category/{category}")] // 新的路由，例如: GET /api/notes/category/日記
    public async Task<IActionResult> GetByCategory(string category, int pageNumber, int pageSize)
    {
        var notes = await _noteService.GetNotesByCategoryPagedAsync(category, pageNumber, pageSize);
        return Ok(notes);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string query)
    {
        var notes = await _noteService.SearchNotesAsync(query);
        return Ok(notes);
    }

    // POST: /api/notes
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NoteDto newNote)
    {
        if (newNote == null)
        {
            return BadRequest();
        }

        newNote.CreatedAt = DateTime.Now;
        await _noteService.CreateNoteAsync(newNote);

        return CreatedAtAction(nameof(GetById), new { id = newNote.Id }, newNote);
    }
}