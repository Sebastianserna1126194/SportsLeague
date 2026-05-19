using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;

namespace SportsLeague.API.Services
{
    public class MatchEventServices
    {
        public MatchResultResponseDTO RegisterMatchResult(MatchResultRequestDTO dto)
        {
            var result = new MatchResult
            {
                GoalsLocal = dto.GoalsLocal,
                GoalsVisitor = dto.GoalsVisitor,
                Observations = dto.Observations
            };

            return new MatchResultResponseDTO
            {
                GoalsLocal = result.GoalsLocal,
                GoalsVisitor = result.GoalsVisitor,
                Observations = result.Observations
            };
        }

        public GoalResponseDTO RegisterGoal(GoalRequestDTO dto)
        {
            var goal = new Goal
            {
                PlayerId = dto.PlayerId,
                Minute = dto.Minute,
                Type = dto.Type
            };

            return new GoalResponseDTO
            {
                PlayerId = goal.PlayerId,
                Minute = goal.Minute,
                Type = goal.Type
            };
        }

        public CardResponseDTO RegisterCard(CardRequestDTO dto)
        {
            var card = new Card
            {
                PlayerId = dto.PlayerId,
                Minute = dto.Minute,
                Type = dto.Type
            };

            return new CardResponseDTO
            {
                PlayerId = card.PlayerId,
                Minute = card.Minute,
                Type = card.Type
            };
        }
    }
}