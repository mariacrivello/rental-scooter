using rental_scooter.Models;

namespace rental_scooter.Repositories
{
    public interface IStationRepository
    {
        Task<bool> DoesStationExist(int stationId);
        Task<Station> GetByIdWithScooters(int stationId);
        Task<Station> GetStationByScooterId(int? scooterId);
        Task<IEnumerable<Station>> GetStationsWithAvailableScootersAsync();
    }
}
