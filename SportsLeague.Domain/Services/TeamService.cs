using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly ILogger<TeamService> _logger;

    public TeamService(ITeamRepository teamRepository, ILogger<TeamService> logger)
    {
        _teamRepository = teamRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Team>> GetAllAsync()
    {
        _logger.LogInformation("Consultando todos los equipos");
        return await _teamRepository.GetAllAsync();
    }

    public async Task<Team?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Buscando equipo con ID {TeamId}", id);

        var team = await _teamRepository.GetByIdAsync(id);

        if (team is null)
        {
            _logger.LogWarning("No apareció el equipo con ID {TeamId}", id);
        }

        return team;
    }

    public async Task<Team> CreateAsync(Team team)
    {
        await ValidateUniqueNameAsync(team.Name);

        _logger.LogInformation("Creando equipo {TeamName}", team.Name);
        return await _teamRepository.CreateAsync(team);
    }

    public async Task UpdateAsync(int id, Team team)
    {
        var currentTeam = await GetExistingTeamOrThrowAsync(id);

        if (!string.Equals(currentTeam.Name, team.Name, StringComparison.OrdinalIgnoreCase))
        {
            await ValidateUniqueNameAsync(team.Name);
        }

        currentTeam.Name = team.Name;
        currentTeam.City = team.City;
        currentTeam.Stadium = team.Stadium;
        currentTeam.LogoUrl = team.LogoUrl;
        currentTeam.FoundedDate = team.FoundedDate;

        _logger.LogInformation("Actualizando equipo con ID {TeamId}", id);
        await _teamRepository.UpdateAsync(currentTeam);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _teamRepository.ExistsAsync(id);

        if (!exists)
        {
            _logger.LogWarning("Intenté borrar un equipo que no existe. ID {TeamId}", id);
            throw new KeyNotFoundException($"No se encontró el equipo con ID {id}");
        }

        _logger.LogInformation("Eliminando equipo con ID {TeamId}", id);
        await _teamRepository.DeleteAsync(id);
    }

    private async Task<Team> GetExistingTeamOrThrowAsync(int id)
    {
        var team = await _teamRepository.GetByIdAsync(id);

        if (team is not null)
        {
            return team;
        }

        _logger.LogWarning("No se pudo actualizar porque el equipo {TeamId} no existe", id);
        throw new KeyNotFoundException($"No se encontró el equipo con ID {id}");
    }

    private async Task ValidateUniqueNameAsync(string teamName)
    {
        var teamWithSameName = await _teamRepository.GetByNameAsync(teamName);

        if (teamWithSameName is null)
        {
            return;
        }

        _logger.LogWarning("Ya hay un equipo registrado con el nombre {TeamName}", teamName);
        throw new InvalidOperationException($"Ya existe un equipo con el nombre '{teamName}'");
    }
}
