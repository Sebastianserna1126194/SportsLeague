namespace SportsLeague.API.DTOs.Request

{
    using SportsLeague.Domain.Entities;
    public class GoalRequestDTO
    {
        public int PlayerId { get; set; }
        public int Minute { get; set; }
        public GoalType Type { get; set; }
    }
}