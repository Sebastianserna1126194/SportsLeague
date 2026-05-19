using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.Services;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchLineupController : ControllerBase
    {
        private readonly MatchLineupService _service;

        public MatchLineupController(MatchLineupService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create(MatchLineupRequestDTO dto)
        {
            // Guardamos la alineación usando el servicio
            var result = _service.CreateLineup(dto);
            return Ok(result);
        }

        [HttpGet("{matchId}")]
        public IActionResult GetByMatch(int matchId)
        {
            var list = _service.GetByMatch(matchId);
            return Ok(list);
        }

        [HttpGet("{matchId}/team/{teamId}")]
        public IActionResult GetByMatchAndTeam(int matchId, int teamId)
        {
            var list = _service.GetByMatchAndTeam(matchId, teamId);
            return Ok(list);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.DeleteLineup(id);
            return Ok();
        }
    }
}