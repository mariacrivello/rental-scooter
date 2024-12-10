using rental_scooter.Models;

namespace rental_scooter.Repositories
{
    public interface IRentalHistoryRepository
    {
        Task<List<RentalHistoryEntry>> GetByUserIdentifier(string userIdentifier);
        Task RentScooter(string userId, int scooterId);
        Task ReturnScooter (string userId, int stationId);

    }
}
