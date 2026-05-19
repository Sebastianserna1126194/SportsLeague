namespace SportsLeague.API.DTOs.Request;
using SportsLeague.Domain.Enums;

public class UpdateStatusDTO
{
    public TournamentStatus Status { get; set; }
}
