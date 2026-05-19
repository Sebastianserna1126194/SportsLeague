namespace SportsLeague.API.DTOs.Response
{
    using SportsLeague.Domain.Entities;
    public class GoalResponseDTO
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Minute { get; set; }
        public GoalType Type { get; set; }
    }
}