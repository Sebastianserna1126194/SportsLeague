using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITournamentTeamRepository _tournamentTeamRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ILogger<TournamentService> _logger;

    public TournamentService(
        ITournamentRepository tournamentRepository,
        ITournamentTeamRepository tournamentTeamRepository,
        ITeamRepository teamRepository,
        ILogger<TournamentService> logger)
    {
        _tournamentRepository = tournamentRepository;
        _tournamentTeamRepository = tournamentTeamRepository;
        _teamRepository = teamRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Tournament>> GetAllAsync()
    {
        _logger.LogInformation("Consultando torneos");
        return await _tournamentRepository.GetAllAsync();
    }

    public async Task<Tournament?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Buscando torneo con ID {TournamentId}", id);

        var tournament = await _tournamentRepository.GetByIdWithTeamsAsync(id);

        if (tournament is null)
        {
            _logger.LogWarning("No apareció el torneo con ID {TournamentId}", id);
        }

        return tournament;
    }

    public async Task<Tournament> CreateAsync(Tournament tournament)
    {
        ValidateDateRange(tournament.StartDate, tournament.EndDate);

        tournament.Status = TournamentStatus.Pending;

        _logger.LogInformation("Creando torneo {TournamentName}", tournament.Name);
        return await _tournamentRepository.CreateAsync(tournament);
    }

    public async Task UpdateAsync(int id, Tournament tournament)
    {
        var currentTournament = await GetTournamentOrThrowAsync(id);

        EnsureTournamentIsPending(currentTournament, "editar");
        ValidateDateRange(tournament.StartDate, tournament.EndDate);

        currentTournament.Name = tournament.Name;
        currentTournament.Season = tournament.Season;
        currentTournament.StartDate = tournament.StartDate;
        currentTournament.EndDate = tournament.EndDate;

        _logger.LogInformation("Actualizando torneo con ID {TournamentId}", id);
        await _tournamentRepository.UpdateAsync(currentTournament);
    }

    public async Task DeleteAsync(int id)
    {
        var currentTournament = await GetTournamentOrThrowAsync(id);

        EnsureTournamentIsPending(currentTournament, "eliminar");

        _logger.LogInformation("Eliminando torneo con ID {TournamentId}", id);
        await _tournamentRepository.DeleteAsync(id);
    }

    public async Task UpdateStatusAsync(int id, TournamentStatus newStatus)
    {
        var tournament = await GetTournamentOrThrowAsync(id);

        if (!IsValidTransition(tournament.Status, newStatus))
        {
            throw new InvalidOperationException(
                $"No se puede cambiar de {tournament.Status} a {newStatus}");
        }

        tournament.Status = newStatus;

        _logger.LogInformation(
            "Cambiando estado del torneo {TournamentId} a {NewStatus}",
            id,
            newStatus);

        await _tournamentRepository.UpdateAsync(tournament);
    }

    public async Task RegisterTeamAsync(int tournamentId, int teamId)
    {
        var tournament = await GetTournamentOrThrowAsync(tournamentId);

        EnsureTournamentIsPending(tournament, "inscribir equipos");
        await EnsureTeamExistsAsync(teamId);
        await EnsureTeamIsNotRegisteredAsync(tournamentId, teamId);

        var registration = new TournamentTeam
        {
            TournamentId = tournamentId,
            TeamId = teamId,
            RegisteredAt = DateTime.UtcNow
        };

        _logger.LogInformation(
            "Inscribiendo equipo {TeamId} en el torneo {TournamentId}",
            teamId,
            tournamentId);

        await _tournamentTeamRepository.CreateAsync(registration);
    }

    public async Task<IEnumerable<Team>> GetTeamsByTournamentAsync(int tournamentId)
    {
        await GetTournamentOrThrowAsync(tournamentId);

        var registrations = await _tournamentTeamRepository.GetByTournamentAsync(tournamentId);
        return registrations.Select(registration => registration.Team);
    }

    private async Task<Tournament> GetTournamentOrThrowAsync(int id)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(id);

        if (tournament is not null)
        {
            return tournament;
        }

        throw new KeyNotFoundException($"No se encontró el torneo con ID {id}");
    }

    private async Task EnsureTeamExistsAsync(int teamId)
    {
        var exists = await _teamRepository.ExistsAsync(teamId);

        if (exists)
        {
            return;
        }

        throw new KeyNotFoundException($"No se encontró el equipo con ID {teamId}");
    }

    private async Task EnsureTeamIsNotRegisteredAsync(int tournamentId, int teamId)
    {
        var registration = await _tournamentTeamRepository.GetByTournamentAndTeamAsync(tournamentId, teamId);

        if (registration is null)
        {
            return;
        }

        throw new InvalidOperationException("Este equipo ya está inscrito en el torneo");
    }

    private static void ValidateDateRange(DateTime startDate, DateTime endDate)
    {
        if (endDate > startDate)
        {
            return;
        }

        throw new InvalidOperationException(
            "La fecha de finalización debe ser posterior a la fecha de inicio");
    }

    private static void EnsureTournamentIsPending(Tournament tournament, string action)
    {
        if (tournament.Status == TournamentStatus.Pending)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Solo se puede {action} un torneo cuando está en estado Pending");
    }

    private static bool IsValidTransition(TournamentStatus currentStatus, TournamentStatus newStatus)
    {
        return (currentStatus, newStatus) switch
        {
            (TournamentStatus.Pending, TournamentStatus.InProgress) => true,
            (TournamentStatus.InProgress, TournamentStatus.Finished) => true,
            _ => false
        };
    }
}
