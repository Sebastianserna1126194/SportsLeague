using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;

namespace SportsLeague.API.Interfaces;

public interface ISponsorService
{
    Task<IEnumerable<SponsorResponseDTO>> GetAllAsync();
    Task<SponsorResponseDTO?> GetByIdAsync(int id);
    Task<SponsorResponseDTO> CreateAsync(SponsorRequestDTO request);
    Task<bool> UpdateAsync(int id, SponsorRequestDTO request);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<TournamentSponsorResponseDTO>> GetTournamentsBySponsorIdAsync(int sponsorId);
    Task<TournamentSponsorResponseDTO> AddTournamentToSponsorAsync(int sponsorId, TournamentSponsorRequestDTO request);
    Task<bool> RemoveTournamentFromSponsorAsync(int sponsorId, int tournamentId);
}