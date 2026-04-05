using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IMapper _mapper;

    public PlayerController(IPlayerService playerService, IMapper mapper)
    {
        _playerService = playerService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> GetAll()
    {
        var players = await _playerService.GetAllAsync();
        var response = _mapper.Map<IEnumerable<PlayerResponseDTO>>(players);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlayerResponseDTO>> GetById(int id)
    {
        var player = await _playerService.GetByIdAsync(id);

        if (player is null)
        {
            return NotFound(new { message = $"No encontré el jugador con ID {id}" });
        }

        return Ok(_mapper.Map<PlayerResponseDTO>(player));
    }

    [HttpGet("team/{teamId:int}")]
    public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> GetByTeam(int teamId)
    {
        try
        {
            var players = await _playerService.GetByTeamAsync(teamId);
            var response = _mapper.Map<IEnumerable<PlayerResponseDTO>>(players);

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(BuildMessage(ex.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<PlayerResponseDTO>> Create([FromBody] PlayerRequestDTO request)
    {
        try
        {
            var playerToCreate = _mapper.Map<Player>(request);
            var createdPlayer = await _playerService.CreateAsync(playerToCreate);

            // Lo vuelvo a consultar para devolver el TeamName completo en la respuesta.
            var savedPlayer = await _playerService.GetByIdAsync(createdPlayer.Id);
            var response = _mapper.Map<PlayerResponseDTO>(savedPlayer);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(BuildMessage(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(BuildMessage(ex.Message));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PlayerRequestDTO request)
    {
        try
        {
            var playerToUpdate = _mapper.Map<Player>(request);
            await _playerService.UpdateAsync(id, playerToUpdate);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(BuildMessage(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(BuildMessage(ex.Message));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _playerService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(BuildMessage(ex.Message));
        }
    }

    private static object BuildMessage(string message) => new { message };
}
