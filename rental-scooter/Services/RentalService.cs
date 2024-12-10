
using rental_scooter.Dtos;
using rental_scooter.Models;
using rental_scooter.Repositories;

namespace rental_scooter.Services
{
    public class RentalService : IRentalService
    {
        private readonly IStationRepository stationRepository;
        private readonly IRentalHistoryRepository rentalHistoryRepository;

        public RentalService(IStationRepository stationRepository, IRentalHistoryRepository rentalHistoryRepository)
        {
            this.stationRepository = stationRepository;
            this.rentalHistoryRepository = rentalHistoryRepository;
        }

        public async Task<UserHistoryEntryDto> GetHistoryEntriesByUserIdentifier(string user)
        {
            var historyEntries = await rentalHistoryRepository.GetByUserIdentifier(user);
            var modifications = new WeeklyModifiers();
            if (historyEntries != null)
            {
                modifications = GetModifications(historyEntries);
            }
            var remainingTime = TimeSpan.FromHours(2);

            return new UserHistoryEntryDto
            {
                rentalHistoryEntries = historyEntries,
                HasPenalties = modifications.HasPenalties,
                HasTimeBonuses = modifications.HasTimeBonuses,
                RemainingTime = remainingTime - modifications.WeeklyRemainingTime
            };
        }

        public async Task<IEnumerable<Station>> GetStationsWithAvailableScooters()
        {
            var result = await stationRepository.GetStationsWithAvailableScootersAsync();
            return result;
        }

        public Task<TimeSpan> RentScooter(ScooterRentRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserHistoryEntryDto>> ReturnScooter(ScooterRentRequest request)
        {
            throw new NotImplementedException();
        }

        private WeeklyModifiers GetModifications(List<RentalHistoryEntry> entries)
        {

            var lastWeekEntries = entries.Where(f => f.RentalStartDateTime <= DateTime.UtcNow.AddDays(-7));
            bool penalties = false;
            bool bonus = false;
            TimeSpan totalElapsedTime = TimeSpan.Zero;

            if (lastWeekEntries != null)
            {
                penalties = lastWeekEntries.Any(f => f.TimeSpan > TimeSpan.FromHours(2));
                bonus = lastWeekEntries.Count() > 2 ? true : false;
                totalElapsedTime = lastWeekEntries
                    .Where(entry => entry.TimeSpan.HasValue)
                    .Aggregate(TimeSpan.Zero, (total, entry) => total + entry.TimeSpan.Value);
            }
            return new WeeklyModifiers
            {
                HasTimeBonuses = bonus,
                HasPenalties = penalties,
                WeeklyRemainingTime = totalElapsedTime
            };
        }

    }
}
