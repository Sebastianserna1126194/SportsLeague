using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RefereeController : ControllerBase
{
    private readonly IRefereeService _refereeService;
    private readonly IMapper _mapper;

    public RefereeController(IRefereeService refereeService, IMapper mapper)
    {
        _refereeService = refereeService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RefereeResponseDTO>>> GetAll()
    {
        var referees = await _refereeService.GetAllAsync();
        var response = _mapper.Map<IEnumerable<RefereeResponseDTO>>(referees);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RefereeResponseDTO>> GetById(int id)
    {
        var referee = await _refereeService.GetByIdAsync(id);

        if (referee is null)
        {
            return NotFound(new { message = $"No encontré el árbitro con ID {id}" });
        }

        return Ok(_mapper.Map<RefereeResponseDTO>(referee));
    }

    [HttpPost]
    public async Task<ActionResult<RefereeResponseDTO>> Create([FromBody] RefereeRequestDTO request)
    {
        var refereeToCreate = _mapper.Map<Referee>(request);
        var createdReferee = await _refereeService.CreateAsync(refereeToCreate);
        var response = _mapper.Map<RefereeResponseDTO>(createdReferee);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RefereeRequestDTO request)
    {
        try
        {
            var refereeToUpdate = _mapper.Map<Referee>(request);
            await _refereeService.UpdateAsync(id, refereeToUpdate);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(BuildMessage(ex.Message));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _refereeService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(BuildMessage(ex.Message));
        }
    }

    private static object BuildMessage(string message) => new { message };
}
