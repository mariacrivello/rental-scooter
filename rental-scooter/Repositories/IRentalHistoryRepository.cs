using rental_scooter.Models;

namespace rental_scooter.Repositories
{
    public interface IRentalHistoryRepository
    {
        Task<List<RentalHistoryEntry>> GetByUserIdentifier(string userIdentifier);
        Task<List<RentalHistoryEntry>> GetByUserIdentifierFilteredByDate(string userIdentifier, DateTime startDate, DateTime endDate);
        Task RentScooter(RentalHistoryEntry rentalHistoryEntry);
        Task ReturnScooter(RentalHistoryEntry returnObject);
    }
}
