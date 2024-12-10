using rental_scooter.Models;

namespace rental_scooter.Repositories
{
    public interface IStationRepository
    {
        Task<IEnumerable<Station>> GetStationsWithAvailableScootersAsync();
    }
}
