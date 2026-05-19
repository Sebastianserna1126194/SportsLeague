namespace SportsLeague.API.DTOs.Request
{
    public class MatchResultRequestDTO
    {
        public int GoalsLocal { get; set; }
        public int GoalsVisitor { get; set; }
        public string Observations { get; set; }
    }
}