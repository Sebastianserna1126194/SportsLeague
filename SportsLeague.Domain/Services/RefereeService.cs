using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class RefereeService : IRefereeService
{
    private readonly IRefereeRepository _refereeRepository;
    private readonly ILogger<RefereeService> _logger;

    public RefereeService(IRefereeRepository refereeRepository, ILogger<RefereeService> logger)
    {
        _refereeRepository = refereeRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Referee>> GetAllAsync()
    {
        _logger.LogInformation("Consultando árbitros");
        return await _refereeRepository.GetAllAsync();
    }

    public async Task<Referee?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Buscando árbitro con ID {RefereeId}", id);

        var referee = await _refereeRepository.GetByIdAsync(id);

        if (referee is null)
        {
            _logger.LogWarning("No apareció el árbitro con ID {RefereeId}", id);
        }

        return referee;
    }

    public async Task<Referee> CreateAsync(Referee referee)
    {
        _logger.LogInformation(
            "Creando árbitro {FirstName} {LastName}",
            referee.FirstName,
            referee.LastName);

        return await _refereeRepository.CreateAsync(referee);
    }

    public async Task UpdateAsync(int id, Referee referee)
    {
        var currentReferee = await _refereeRepository.GetByIdAsync(id);

        if (currentReferee is null)
        {
            throw new KeyNotFoundException($"No se encontró el árbitro con ID {id}");
        }

        currentReferee.FirstName = referee.FirstName;
        currentReferee.LastName = referee.LastName;
        currentReferee.Nationality = referee.Nationality;

        _logger.LogInformation("Actualizando árbitro con ID {RefereeId}", id);
        await _refereeRepository.UpdateAsync(currentReferee);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _refereeRepository.ExistsAsync(id);

        if (!exists)
        {
            throw new KeyNotFoundException($"No se encontró el árbitro con ID {id}");
        }

        _logger.LogInformation("Eliminando árbitro con ID {RefereeId}", id);
        await _refereeRepository.DeleteAsync(id);
    }
}
