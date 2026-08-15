using Microsoft.AspNetCore.Mvc;
using GameApi.Models;
using GameApi.Data; 

namespace GameApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly AppDbContext _context;

    // 透過 Constructor 注入 DbContext
    public PlayersController(AppDbContext context)
    {
        _context = context;
    }
    //private readonly List<Player> _players;

    // public PlayersController()
    // {
    //     _players = new List<Player>
    //     {
    //         new Player { Id = 1, Name = "Player1", Score = 100, Level = 5 },
    //         new Player { Id = 2, Name = "Player2", Score = 200, Level = 10 },
    //         new Player { Id = 3, Name = "Player3", Score = 150, Level = 7 }
    //     };
    // }
// GET: /api/players
    [HttpGet]
    public IActionResult GetAll()
    {
        var players = _context.Players
            .Select(p => new PlayerDto
            {
                Id = p.Id,
                Name = p.Name,
                Score = p.Score,
                Level = p.Level
            })
            .ToList();
        return Ok(players);
    }


    [HttpGet("{id}")]
    public IActionResult GetPlayer(int id)
    {
        var player = _context.Players.FirstOrDefault(p => p.Id == id);
        if (player == null)
        {
            return NotFound();
        }
        return Ok(player);
    }

    // POST: /api/players
    [HttpPost]
[HttpPost]
public IActionResult Create([FromBody] PlayerDto newPlayerDto)
{
    if (newPlayerDto == null)
    {
        return BadRequest("玩家資料不得為空。");
    }

    var player = new Player
    {
        Name = newPlayerDto.Name,
        Score = newPlayerDto.Score,
        Level = newPlayerDto.Level
    };

    _context.Players.Add(player);
    _context.SaveChanges();

    var resultDto = new PlayerDto
    {
        Id = player.Id,
        Name = player.Name,
        Score = player.Score,
        Level = player.Level
    };

    return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, resultDto);
}

    // PUT: /api/players/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Player updatedPlayer)
    {
        var player = _context.Players.FirstOrDefault(p => p.Id == id);
        if (player == null)
            return NotFound($"找不到 ID {id} 嘅玩家");
        
        player.Name = updatedPlayer.Name;
        player.Score = updatedPlayer.Score;
        player.Level = updatedPlayer.Level;
        
        _context.SaveChanges();
        return Ok(player);
    }
}