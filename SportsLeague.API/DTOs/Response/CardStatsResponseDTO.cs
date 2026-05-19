namespace SportsLeague.API.DTOs.Response

{
    
    public class CardStatsResponseDTO
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int TeamId { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int TotalCards { get; set; }
    }
}