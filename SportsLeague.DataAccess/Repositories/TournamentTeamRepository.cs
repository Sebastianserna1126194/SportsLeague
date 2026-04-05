using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class TournamentTeamRepository : GenericRepository<TournamentTeam>, ITournamentTeamRepository
{
    public TournamentTeamRepository(LeagueDbContext context) : base(context)
    {
    }

    public async Task<TournamentTeam?> GetByTournamentAndTeamAsync(int tournamentId, int teamId)
    {
        return await DbSet
            .FirstOrDefaultAsync(link => link.TournamentId == tournamentId && link.TeamId == teamId);
    }

    public async Task<IEnumerable<TournamentTeam>> GetByTournamentAsync(int tournamentId)
    {
        return await DbSet
            .Where(link => link.TournamentId == tournamentId)
            .Include(link => link.Team)
            .ToListAsync();
    }
}
