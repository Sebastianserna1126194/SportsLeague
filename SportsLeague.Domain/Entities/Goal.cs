namespace SportsLeague.Domain.Entities
{
    public enum GoalType { Normal, Penalty, OwnGoal }

    public class Goal
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Minute { get; set; }
        public GoalType Type { get; set; }

        public Match Match { get; set; }
        public Player Player { get; set; }
    }
}