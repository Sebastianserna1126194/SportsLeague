using SportsLeague.API.DTOs.Response;

namespace SportsLeague.API.Services
{
    public class StandingService
    {
        public IEnumerable<StandingResponseDTO> GetStandings()
        {
            return new List<StandingResponseDTO>();
        }

        public IEnumerable<TopScorerResponseDTO> GetTopScorers()
        {
            return new List<TopScorerResponseDTO>();
        }

        public IEnumerable<CardStatsResponseDTO> GetCardStats()
        {
            return new List<CardStatsResponseDTO>();
        }
    }
}
    

    
