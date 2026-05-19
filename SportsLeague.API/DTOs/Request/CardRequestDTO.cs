namespace SportsLeague.API.DTOs.Request
{
    using SportsLeague.Domain.Entities;
    public class CardRequestDTO
    {
        public int PlayerId { get; set; }
        public int Minute { get; set; }
        public CardType Type { get; set; }
    }
}