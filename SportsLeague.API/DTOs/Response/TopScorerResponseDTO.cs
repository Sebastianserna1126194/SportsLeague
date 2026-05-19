namespace SportsLeague.API.DTOs.Response
{
    public class TopScorerResponseDTO
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int TeamId { get; set; }
        public int Goals { get; set; }
        public int Matches { get; set; }
    }
}