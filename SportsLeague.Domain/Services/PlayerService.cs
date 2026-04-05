using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(
        IPlayerRepository playerRepository,
        ITeamRepository teamRepository,
        ILogger<PlayerService> logger)
    {
        _playerRepository = playerRepository;
        _teamRepository = teamRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Player>> GetAllAsync()
    {
        _logger.LogInformation("Consultando jugadores con su equipo");
        return await _playerRepository.GetAllWithTeamAsync();
    }

    public async Task<Player?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Buscando jugador con ID {PlayerId}", id);

        var player = await _playerRepository.GetByIdWithTeamAsync(id);

        if (player is null)
        {
            _logger.LogWarning("No apareció el jugador con ID {PlayerId}", id);
        }

        return player;
    }

    public async Task<IEnumerable<Player>> GetByTeamAsync(int teamId)
    {
        await EnsureTeamExistsAsync(teamId);

        _logger.LogInformation("Consultando jugadores del equipo {TeamId}", teamId);
        return await _playerRepository.GetByTeamAsync(teamId);
    }

    public async Task<Player> CreateAsync(Player player)
    {
        await EnsureTeamExistsAsync(player.TeamId);
        await EnsureNumberIsAvailableAsync(player.TeamId, player.Number);

        _logger.LogInformation(
            "Creando jugador {FirstName} {LastName}",
            player.FirstName,
            player.LastName);

        return await _playerRepository.CreateAsync(player);
    }

    public async Task UpdateAsync(int id, Player player)
    {
        var currentPlayer = await GetExistingPlayerOrThrowAsync(id);

        await EnsureTeamExistsAsync(player.TeamId);

        var teamChanged = currentPlayer.TeamId != player.TeamId;
        var numberChanged = currentPlayer.Number != player.Number;

        if (teamChanged || numberChanged)
        {
            await EnsureNumberIsAvailableAsync(player.TeamId, player.Number, id);
        }

        currentPlayer.FirstName = player.FirstName;
        currentPlayer.LastName = player.LastName;
        currentPlayer.BirthDate = player.BirthDate;
        currentPlayer.Number = player.Number;
        currentPlayer.Position = player.Position;
        currentPlayer.TeamId = player.TeamId;

        _logger.LogInformation("Actualizando jugador con ID {PlayerId}", id);
        await _playerRepository.UpdateAsync(currentPlayer);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _playerRepository.ExistsAsync(id);

        if (!exists)
        {
            throw new KeyNotFoundException($"No se encontró el jugador con ID {id}");
        }

        _logger.LogInformation("Eliminando jugador con ID {PlayerId}", id);
        await _playerRepository.DeleteAsync(id);
    }

    private async Task<Player> GetExistingPlayerOrThrowAsync(int id)
    {
        var player = await _playerRepository.GetByIdAsync(id);

        if (player is not null)
        {
            return player;
        }

        throw new KeyNotFoundException($"No se encontró el jugador con ID {id}");
    }

    private async Task EnsureTeamExistsAsync(int teamId)
    {
        var exists = await _teamRepository.ExistsAsync(teamId);

        if (exists)
        {
            return;
        }

        _logger.LogWarning("No existe el equipo con ID {TeamId}", teamId);
        throw new KeyNotFoundException($"No se encontró el equipo con ID {teamId}");
    }

    private async Task EnsureNumberIsAvailableAsync(int teamId, int number, int? currentPlayerId = null)
    {
        var playerWithSameNumber = await _playerRepository.GetByTeamAndNumberAsync(teamId, number);

        if (playerWithSameNumber is null || playerWithSameNumber.Id == currentPlayerId)
        {
            return;
        }

        _logger.LogWarning(
            "El número {Number} ya lo tiene otro jugador en el equipo {TeamId}",
            number,
            teamId);

        throw new InvalidOperationException($"El número {number} ya está en uso en este equipo");
    }
}
