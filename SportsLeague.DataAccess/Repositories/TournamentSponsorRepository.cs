using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
{
    private readonly LeagueDbContext _context;

    public TournamentSponsorRepository(LeagueDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
    {
        return await _context.TournamentSponsors
            .Include(tournamentSponsor => tournamentSponsor.Tournament)
            .Include(tournamentSponsor => tournamentSponsor.Sponsor)
            .Where(tournamentSponsor => tournamentSponsor.SponsorId == sponsorId)
            .ToListAsync();
    }

    public async Task<bool> ExistsRelationAsync(int sponsorId, int tournamentId)
    {
        return await _context.TournamentSponsors
            .AnyAsync(tournamentSponsor =>
                tournamentSponsor.SponsorId == sponsorId &&
                tournamentSponsor.TournamentId == tournamentId);
    }

    public async Task<TournamentSponsor?> GetBySponsorAndTournamentAsync(int sponsorId, int tournamentId)
    {
        return await _context.TournamentSponsors
            .FirstOrDefaultAsync(tournamentSponsor =>
                tournamentSponsor.SponsorId == sponsorId &&
                tournamentSponsor.TournamentId == tournamentId);
    }
}