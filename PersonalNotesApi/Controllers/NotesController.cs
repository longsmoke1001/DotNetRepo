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

    // 從 JWT Token 取得當前用戶嘅 UserId（冇 claim 就回傳 null，避免攞到其他用戶嘅筆記）
    private int? CurrentUserId
    {
        get
        {
            var raw = User.FindFirst("sub")?.Value
                   ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(raw, out var id) ? id : null;
        }
    }

    // GET: /api/notes
    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
    {
        var userId = CurrentUserId;
        if (userId is null) return Unauthorized();
        var notes = await _noteService.GetNotesPagedAsync(pageNumber, pageSize, userId.Value);
        return Ok(notes);
    }

    // GET: /api/notes/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = CurrentUserId;
        if (userId is null) return Unauthorized();
        var note = await _noteService.GetNoteByIdAsync(id, userId.Value);
        if (note == null)
        {
            return NotFound();
        }
        return Ok(note);
    }

    [HttpGet("category/{category}")] // 新的路由，例如: GET /api/notes/category/日記
    public async Task<IActionResult> GetByCategory(string category, int pageNumber, int pageSize)
    {
        var userId = CurrentUserId;
        if (userId is null) return Unauthorized();
        var notes = await _noteService.GetNotesByCategoryPagedAsync(category, pageNumber, pageSize, userId.Value);
        return Ok(notes);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string query)
    {
        var userId = CurrentUserId;
        if (userId is null) return Unauthorized();
        var notes = await _noteService.SearchNotesAsync(query, userId.Value);
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

        var userId = CurrentUserId;
        if (userId is null) return Unauthorized();

        newNote.CreatedAt = DateTime.Now;
        var created = await _noteService.CreateNoteAsync(newNote, userId.Value);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = CurrentUserId;
        if (userId is null) return Unauthorized();
        var result = await _noteService.DeleteAsync(id, userId.Value);
        if (!result) return NotFound();
        return NoContent();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] NoteDto dto)
    {
        var userId = CurrentUserId;
        if (userId is null) return Unauthorized();
        var result = await _noteService.UpdateAsync(id, dto, userId.Value);
        if (!result) return NotFound();
        return NoContent();
    }
}