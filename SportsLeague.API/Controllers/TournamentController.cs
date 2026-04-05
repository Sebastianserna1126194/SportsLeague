using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TournamentController : ControllerBase
{
    private readonly ITournamentService _tournamentService;
    private readonly IMapper _mapper;
    private readonly ILogger<TournamentController> _logger;

    public TournamentController(
        ITournamentService tournamentService,
        IMapper mapper,
        ILogger<TournamentController> logger)
    {
        _tournamentService = tournamentService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentResponseDTO>>> GetAll()
    {
        var tournaments = await _tournamentService.GetAllAsync();
        var response = _mapper.Map<IEnumerable<TournamentResponseDTO>>(tournaments);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TournamentResponseDTO>> GetById(int id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);

        if (tournament is null)
        {
            return NotFound(new { message = $"No encontré el torneo con ID {id}" });
        }

        return Ok(_mapper.Map<TournamentResponseDTO>(tournament));
    }

    [HttpPost]
    public async Task<ActionResult<TournamentResponseDTO>> Create([FromBody] TournamentRequestDTO request)
    {
        try
        {
            var tournamentToCreate = _mapper.Map<Tournament>(request);
            var createdTournament = await _tournamentService.CreateAsync(tournamentToCreate);
            var response = _mapper.Map<TournamentResponseDTO>(createdTournament);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "No pude crear el torneo");
            return BadRequest(BuildMessage(ex.Message));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TournamentRequestDTO request)
    {
        try
        {
            var tournamentToUpdate = _mapper.Map<Tournament>(request);
            await _tournamentService.UpdateAsync(id, tournamentToUpdate);

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
            await _tournamentService.DeleteAsync(id);
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

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDTO request)
    {
        try
        {
            await _tournamentService.UpdateStatusAsync(id, request.Status);
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

    [HttpPost("{id:int}/teams")]
    public async Task<IActionResult> RegisterTeam(int id, [FromBody] RegisterTeamDTO request)
    {
        try
        {
            await _tournamentService.RegisterTeamAsync(id, request.TeamId);
            return Ok(new { message = "Equipo inscrito sin lío" });
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

    [HttpGet("{id:int}/teams")]
    public async Task<ActionResult<IEnumerable<TeamResponseDTO>>> GetTeams(int id)
    {
        try
        {
            var teams = await _tournamentService.GetTeamsByTournamentAsync(id);
            var response = _mapper.Map<IEnumerable<TeamResponseDTO>>(teams);

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(BuildMessage(ex.Message));
        }
    }

    private static object BuildMessage(string message) => new { message };
}
