namespace SportsLeague.Domain.Entities
{
    public enum MatchStatus { Scheduled, InProgress, Finished, Cancelled }

    public class Match
    {
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int TournamentId { get; set; }
        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; }

        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public Tournament Tournament { get; set; }
    }
}