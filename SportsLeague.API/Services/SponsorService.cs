using AutoMapper;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.API.Interfaces;
using System.Net.Mail;

namespace SportsLeague.API.Services;

public class SponsorService : ISponsorService
{
    private readonly ISponsorRepository _sponsorRepository;
    private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IMapper _mapper;

    public SponsorService(
        ISponsorRepository sponsorRepository,
        ITournamentSponsorRepository tournamentSponsorRepository,
        ITournamentRepository tournamentRepository,
        IMapper mapper)
    {
        _sponsorRepository = sponsorRepository;
        _tournamentSponsorRepository = tournamentSponsorRepository;
        _tournamentRepository = tournamentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SponsorResponseDTO>> GetAllAsync()
    {
        var sponsors = await _sponsorRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors);
    }

    public async Task<SponsorResponseDTO?> GetByIdAsync(int id)
    {
        var sponsor = await _sponsorRepository.GetByIdAsync(id);

        if (sponsor == null)
        {
            return null;
        }

        return _mapper.Map<SponsorResponseDTO>(sponsor);
    }

    public async Task<SponsorResponseDTO> CreateAsync(SponsorRequestDTO request)
    {
        var exists = await _sponsorRepository.ExistsByNameAsync(request.Name);

        if (exists)
        {
            throw new InvalidOperationException("Ya existe un sponsor con ese nombre");
        }

        if (!IsValidEmail(request.ContactEmail))
        {
            throw new InvalidOperationException("El email no tiene un formato válido");
        }

        var sponsor = _mapper.Map<Sponsor>(request);
        sponsor.CreatedAt = DateTime.UtcNow;

        var createdSponsor = await _sponsorRepository.CreateAsync(sponsor);

        return _mapper.Map<SponsorResponseDTO>(createdSponsor);
    }

    public async Task<bool> UpdateAsync(int id, SponsorRequestDTO request)
    {
        var sponsor = await _sponsorRepository.GetByIdAsync(id);

        if (sponsor == null)
        {
            return false;
        }

        if (!string.Equals(sponsor.Name, request.Name, StringComparison.OrdinalIgnoreCase))
        {
            var exists = await _sponsorRepository.ExistsByNameAsync(request.Name);

            if (exists)
            {
                throw new InvalidOperationException("Ya existe un sponsor con ese nombre");
            }
        }

        if (!IsValidEmail(request.ContactEmail))
        {
            throw new InvalidOperationException("El email no tiene un formato válido");
        }

        sponsor.Name = request.Name;
        sponsor.ContactEmail = request.ContactEmail;
        sponsor.Phone = request.Phone;
        sponsor.WebsiteUrl = request.WebsiteUrl;
        sponsor.Category = request.Category;
        sponsor.UpdatedAt = DateTime.UtcNow;

        await _sponsorRepository.UpdateAsync(sponsor);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sponsor = await _sponsorRepository.GetByIdAsync(id);

        if (sponsor == null)
        {
            return false;
        }

        await _sponsorRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<TournamentSponsorResponseDTO>> GetTournamentsBySponsorIdAsync(int sponsorId)
    {
        var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);

        if (sponsor == null)
        {
            throw new KeyNotFoundException("No se encontró el sponsor");
        }

        var relations = await _tournamentSponsorRepository.GetBySponsorIdAsync(sponsorId);

        return relations.Select(relation => new TournamentSponsorResponseDTO
        {
            Id = relation.Id,
            TournamentId = relation.TournamentId,
            TournamentName = relation.Tournament.Name,
            SponsorId = relation.SponsorId,
            SponsorName = relation.Sponsor.Name,
            ContractAmount = relation.ContractAmount,
            JoinedAt = relation.JoinedAt
        });
    }

    public async Task<TournamentSponsorResponseDTO> AddTournamentToSponsorAsync(int sponsorId, TournamentSponsorRequestDTO request)
    {
        var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);

        if (sponsor == null)
        {
            throw new KeyNotFoundException("No se encontró el sponsor");
        }

        var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId);

        if (tournament == null)
        {
            throw new KeyNotFoundException("No se encontró el torneo");
        }

        if (request.ContractAmount <= 0)
        {
            throw new InvalidOperationException("El monto del contrato debe ser mayor a 0");
        }

        var existsRelation = await _tournamentSponsorRepository.ExistsRelationAsync(sponsorId, request.TournamentId);

        if (existsRelation)
        {
            throw new InvalidOperationException("Ese sponsor ya está vinculado a ese torneo");
        }

        var relation = new TournamentSponsor
        {
            SponsorId = sponsorId,
            TournamentId = request.TournamentId,
            ContractAmount = request.ContractAmount,
            JoinedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        var createdRelation = await _tournamentSponsorRepository.CreateAsync(relation);

        return new TournamentSponsorResponseDTO
        {
            Id = createdRelation.Id,
            TournamentId = tournament.Id,
            TournamentName = tournament.Name,
            SponsorId = sponsor.Id,
            SponsorName = sponsor.Name,
            ContractAmount = createdRelation.ContractAmount,
            JoinedAt = createdRelation.JoinedAt
        };
    }

    public async Task<bool> RemoveTournamentFromSponsorAsync(int sponsorId, int tournamentId)
    {
        var relation = await _tournamentSponsorRepository.GetBySponsorAndTournamentAsync(sponsorId, tournamentId);

        if (relation == null)
        {
            return false;
        }

        await _tournamentSponsorRepository.DeleteAsync(relation.Id);
        return true;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }
}