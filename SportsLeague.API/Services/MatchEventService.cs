namespace SportsLeague.API.Services;
using SportsLeague.Domain.Entities; 
using SportsLeague.API.DTOs.Request; 

    public class MatchEventService
    {
       
        public MatchResult RegisterMatchResult(MatchResult result)
        {
            // Guardamos resultado en la BD
            return result;
        }

        public Goal RegisterGoal(Goal goal)
        {
            return goal;
        }

        public Card RegisterCard(Card card)
        {
            return card;
        }
    }
