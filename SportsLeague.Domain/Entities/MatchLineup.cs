namespace SportsLeague.Domain.Entities
{
    public class MatchLineup
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public bool IsStarter { get; set; }
        public string Position { get; set; }

        public Match Match { get; set; }
        public Player Player { get; set; }
    }
}