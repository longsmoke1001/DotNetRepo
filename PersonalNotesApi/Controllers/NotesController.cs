using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalNotesApi.DTOs;
using PersonalNotesApi.Models;
using PersonalNotesApi.Services;
//using PersonalNotesApi.Data;

namespace PersonalNotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;  // 改用 Service
    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }

    // 從 JWT Token 取得當前用戶嘅 UserId
    private int CurrentUserId =>
        int.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

    // GET: /api/notes
    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
    {
        var notes = await _noteService.GetNotesPagedAsync(pageNumber, pageSize, CurrentUserId);
        return Ok(notes);
    }

    // GET: /api/notes/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var note = await _noteService.GetNoteByIdAsync(id, CurrentUserId);
        if (note == null)
        {
            return NotFound();
        }
        return Ok(note);
    }

    [HttpGet("category/{category}")] // 新的路由，例如: GET /api/notes/category/日記
    public async Task<IActionResult> GetByCategory(string category, int pageNumber, int pageSize)
    {
        var notes = await _noteService.GetNotesByCategoryPagedAsync(category, pageNumber, pageSize, CurrentUserId);
        return Ok(notes);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string query)
    {
        var notes = await _noteService.SearchNotesAsync(query, CurrentUserId);
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
        await _noteService.CreateNoteAsync(newNote, CurrentUserId);

        return CreatedAtAction(nameof(GetById), new { id = newNote.Id }, newNote);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _noteService.DeleteAsync(id, CurrentUserId);
        if (!result) return NotFound();
        return NoContent();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] NoteDto dto)
    {
        var result = await _noteService.UpdateAsync(id, dto, CurrentUserId);
        if (!result) return NotFound();
        return NoContent();
    }
}