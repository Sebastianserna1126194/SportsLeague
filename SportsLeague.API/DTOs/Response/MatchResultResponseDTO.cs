namespace SportsLeague.API.DTOs.Response
{
    public class MatchResultResponseDTO
    {
        public int Id { get; set; }
        public int GoalsLocal { get; set; }
        public int GoalsVisitor { get; set; }
        public string Observations { get; set; }
    }
}