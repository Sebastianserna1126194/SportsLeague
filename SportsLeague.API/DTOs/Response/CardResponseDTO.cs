namespace SportsLeague.API.DTOs.Response
{
    using SportsLeague.Domain.Entities;
    public class CardResponseDTO
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Minute { get; set; }
        public CardType Type { get; set; }
    }
}