
using rental_scooter.Dtos;
using rental_scooter.Models;

namespace rental_scooter.Services
{
    public interface IRentalService
    {
        Task<UserHistoryEntryDto> GetHistoryEntriesByUserIdentifier(string user);
        Task<UserRentDto> GetHistoryEntriesByUserIdentifierFilteredByDate(string user, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Station>> GetStationsWithAvailableScooters();
        Task<TimeSpan> RentScooter(ScooterRentRequest request);
        Task<TimeSpan> ReturnScooter(ScooterReturnRequest request);
    }
}
