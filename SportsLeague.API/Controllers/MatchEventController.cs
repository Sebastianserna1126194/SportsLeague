using Microsoft.AspNetCore.Mvc;
namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchEventController : ControllerBase
    {
        private readonly MatchEventService _service;

        public MatchEventController(MatchEventService service)
        {
            _service = service;
        }

        [HttpPost("result")]
        public IActionResult AddResult(MatchResultRequestDTO dto)
        {
            return Ok(dto);
        }

        [HttpPost("goal")]
        public IActionResult AddGoal(GoalRequestDTO dto)
        {
            return Ok(dto);
        }

        [HttpPost("card")]
        public IActionResult AddCard(CardRequestDTO dto)
        {
            return Ok(dto);
        }
    }
}