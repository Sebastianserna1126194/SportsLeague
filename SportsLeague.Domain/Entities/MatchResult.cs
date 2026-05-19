namespace SportsLeague.Domain.Entities
{
    public class MatchResult
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int GoalsLocal { get; set; }
        public int GoalsVisitor { get; set; }
        public string Observations { get; set; }

        public Match Match { get; set; }
    }
}