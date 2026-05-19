namespace SportsLeague.Domain.Entities
{
    public enum CardType { Yellow, Red }

    public class Card
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Minute { get; set; }
        public CardType Type { get; set; }

        public Match Match { get; set; }
        public Player Player { get; set; }
    }
}