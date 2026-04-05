using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.Interfaces;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorController : ControllerBase
{
    private readonly ISponsorService _sponsorService;

    public SponsorController(ISponsorService sponsorService)
    {
        _sponsorService = sponsorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sponsors = await _sponsorService.GetAllAsync();
        return Ok(sponsors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sponsor = await _sponsorService.GetByIdAsync(id);

        if (sponsor == null)
        {
            return NotFound("No se encontró el sponsor");
        }

        return Ok(sponsor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SponsorRequestDTO request)
    {
        try
        {
            var createdSponsor = await _sponsorService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdSponsor.Id }, createdSponsor);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] SponsorRequestDTO request)
    {
        try
        {
            var updated = await _sponsorService.UpdateAsync(id, request);

            if (!updated)
            {
                return NotFound("No se encontró el sponsor");
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _sponsorService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound("No se encontró el sponsor");
        }

        return NoContent();
    }

    [HttpGet("{id}/tournaments")]
    public async Task<IActionResult> GetTournamentsBySponsorId(int id)
    {
        try
        {
            var tournaments = await _sponsorService.GetTournamentsBySponsorIdAsync(id);
            return Ok(tournaments);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{id}/tournaments")]
    public async Task<IActionResult> AddTournamentToSponsor(int id, [FromBody] TournamentSponsorRequestDTO request)
    {
        try
        {
            var result = await _sponsorService.AddTournamentToSponsorAsync(id, request);
            return Created(string.Empty, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id}/tournaments/{tournamentId}")]
    public async Task<IActionResult> RemoveTournamentFromSponsor(int id, int tournamentId)
    {
        var deleted = await _sponsorService.RemoveTournamentFromSponsorAsync(id, tournamentId);

        if (!deleted)
        {
            return NotFound("No se encontró la relación entre el sponsor y el torneo");
        }

        return NoContent();
    }
}