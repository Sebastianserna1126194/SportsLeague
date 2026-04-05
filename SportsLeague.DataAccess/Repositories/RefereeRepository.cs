using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class RefereeRepository : GenericRepository<Referee>, IRefereeRepository
{
    public RefereeRepository(LeagueDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Referee>> GetByNationalityAsync(string nationality)
    {
        var normalizedNationality = nationality.Trim().ToLower();

        return await DbSet
            .Where(referee => referee.Nationality.ToLower() == normalizedNationality)
            .ToListAsync();
    }
}
