using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IMapper _mapper;

    public TeamController(ITeamService teamService, IMapper mapper)
    {
        _teamService = teamService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamResponseDTO>>> GetAll()
    {
        var teams = await _teamService.GetAllAsync();
        var response = _mapper.Map<IEnumerable<TeamResponseDTO>>(teams);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TeamResponseDTO>> GetById(int id)
    {
        var team = await _teamService.GetByIdAsync(id);

        if (team is null)
        {
            return NotFound(new { message = $"No encontré el equipo con ID {id}" });
        }

        return Ok(_mapper.Map<TeamResponseDTO>(team));
    }

    [HttpPost]
    public async Task<ActionResult<TeamResponseDTO>> Create([FromBody] TeamRequestDTO request)
    {
        try
        {
            var teamToCreate = _mapper.Map<Team>(request);
            var createdTeam = await _teamService.CreateAsync(teamToCreate);
            var response = _mapper.Map<TeamResponseDTO>(createdTeam);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(BuildMessage(ex.Message));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TeamRequestDTO request)
    {
        try
        {
            var teamToUpdate = _mapper.Map<Team>(request);
            await _teamService.UpdateAsync(id, teamToUpdate);

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
            await _teamService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(BuildMessage(ex.Message));
        }
    }

    private static object BuildMessage(string message) => new { message };
}
