using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
{
    public PlayerRepository(LeagueDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Player>> GetByTeamAsync(int teamId)
    {
        return await DbSet
            .Where(player => player.TeamId == teamId)
            .Include(player => player.Team)
            .ToListAsync();
    }

    public async Task<Player?> GetByTeamAndNumberAsync(int teamId, int number)
    {
        return await DbSet
            .FirstOrDefaultAsync(player => player.TeamId == teamId && player.Number == number);
    }

    public async Task<IEnumerable<Player>> GetAllWithTeamAsync()
    {
        return await DbSet
            .Include(player => player.Team)
            .ToListAsync();
    }

    public async Task<Player?> GetByIdWithTeamAsync(int id)
    {
        return await DbSet
            .Include(player => player.Team)
            .FirstOrDefaultAsync(player => player.Id == id);
    }
}
