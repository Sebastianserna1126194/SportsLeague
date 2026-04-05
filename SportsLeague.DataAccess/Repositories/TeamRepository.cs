using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class TeamRepository : GenericRepository<Team>, ITeamRepository
{
    public TeamRepository(LeagueDbContext context) : base(context)
    {
    }

    public async Task<Team?> GetByNameAsync(string name)
    {
        var normalizedName = name.Trim().ToLower();

        return await DbSet
            .FirstOrDefaultAsync(team => team.Name.ToLower() == normalizedName);
    }

    public async Task<IEnumerable<Team>> GetByCityAsync(string city)
    {
        var normalizedCity = city.Trim().ToLower();

        return await DbSet
            .Where(team => team.City.ToLower() == normalizedCity)
            .ToListAsync();
    }
}
