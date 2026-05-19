namespace SportsLeague.API.DTOs.Response
{
    public class StandingResponseDTO
    {
        public int Position { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int PJ { get; set; }
        public int PG { get; set; }
        public int PE { get; set; }
        public int PP { get; set; }
        public int GF { get; set; }
        public int GA { get; set; }
        public int GD { get; set; }
        public int Points { get; set; }
    }
}