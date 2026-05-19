using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;

namespace SportsLeague.API.Services
{
    public class MatchLineupService
    {
        // Crea una alineación
        public MatchLineupResponseDTO CreateLineup(MatchLineupRequestDTO dto)
        {
            var lineup = new MatchLineup
            {
                PlayerId = dto.PlayerId,
                IsStarter = dto.IsStarter,
                Position = dto.Position
            };

            // Aquí podrías guardar 'lineup' en la base de datos

            return new MatchLineupResponseDTO
            {
                PlayerId = lineup.PlayerId,
                IsStarter = lineup.IsStarter,
                Position = lineup.Position
            };
        }

        // Devuelve la alineación por match
        public List<MatchLineupResponseDTO> GetByMatch(int matchId)
        {
            return new List<MatchLineupResponseDTO>();
        }

        public List<MatchLineupResponseDTO> GetByMatchAndTeam(int matchId, int teamId)
        {
            return new List<MatchLineupResponseDTO>();
        }

        public void DeleteLineup(int id)
        {
            // Eliminar de la BD
        }
    }
}