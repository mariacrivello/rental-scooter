using rental_scooter.Models;

namespace rental_scooter.Repositories
{
    public interface IStationRepository
    {
        Task<bool> DoesScooterExist(int scooterId);
        Task<bool> DoesStationExist(int stationId);
        Task<Station> GetByScooterId(int? scooterId);
        Task<IEnumerable<Station>> GetStationsWithAvailableScootersAsync();
    }
}
