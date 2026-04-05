using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
{
    Task<List<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);
    Task<bool> ExistsRelationAsync(int sponsorId, int tournamentId);
    Task<TournamentSponsor?> GetBySponsorAndTournamentAsync(int sponsorId, int tournamentId);
}