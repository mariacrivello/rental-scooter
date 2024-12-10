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
            var remainingTime = TimeSpan.FromHours(2);

            if (historyEntries != null)
            {
                modifications = GetModifications(historyEntries);
            }

            return new UserHistoryEntryDto
            {
                rentalHistoryEntries = historyEntries,
                HasPenalties = modifications.HasPenalties,
                HasTimeBonuses = modifications.HasTimeBonuses,
                RemainingTime = remainingTime - modifications.WeeklyElapsedTime
            };
        }

        public async Task<IEnumerable<Station>> GetStationsWithAvailableScooters()
        {
            var result = await stationRepository.GetStationsWithAvailableScootersAsync();
            return result;
        }

        public async Task RentScooter(ScooterRentRequest request)
        {
           await ValidateDataOnAdd(request);

            var rentObject = new RentalHistoryEntry
            {
                UserIdentifier = request.UserIdentifier,
                
                ScooterId = request.ScooterId,
                Scooter = null, 

                PickUpStationId = request.StationId,
                PickUpStation = null,
                
                RentalStartDateTime = DateTime.Now,
            };
            
            await rentalHistoryRepository.RentScooter(rentObject);
        }



        public Task<IList<UserHistoryEntryDto>> ReturnScooter(ScooterRentRequest request)
        {
            throw new NotImplementedException();
        }

        private async Task ValidateDataOnAdd(ScooterRentRequest request)
        {
            var doesScooterExist = await stationRepository.DoesScooterExist(request.ScooterId);
            if (doesScooterExist == false) { throw new InvalidOperationException("No se encontro el scooter"); }
            
            var doesStationExist = await stationRepository.DoesStationExist(request.StationId);
            if (doesStationExist == false) { throw new InvalidOperationException("No se encontro la estacion"); }

            var userRelatedHistoryEntries = await rentalHistoryRepository.GetByUserIdentifier(request.UserIdentifier);
            if (userRelatedHistoryEntries != null)
            {
                var modifications = GetModifications(userRelatedHistoryEntries);
                if (userRelatedHistoryEntries.LastOrDefault().DropOffStation is null)
                {
                    throw new InvalidOperationException("el usuario tiene viajes pendientes");
                }

                else if (modifications.WeeklyRemainingTime < TimeSpan.FromHours(0))
                {
                    throw new InvalidOperationException("El usuario no posee tiempo restante");
                }
            }
        }

        private WeeklyModifiers GetModifications(List<RentalHistoryEntry> entries)
        {

            var lastWeekEntries = entries.Where(f => f.RentalStartDateTime <= DateTime.UtcNow.AddDays(-7));
            bool penalties = false;
            bool bonus = false;
            TimeSpan totalElapsedTime = TimeSpan.Zero;
            var remainingTime = TimeSpan.FromHours(2);


            if (lastWeekEntries != null)
            {
                penalties = lastWeekEntries.Any(f => f.TimeSpan > TimeSpan.FromHours(2));
                bonus = lastWeekEntries.Count() > 2 ? true : false;
                totalElapsedTime = lastWeekEntries
                    .Where(entry => entry.TimeSpan.HasValue)
                    .Aggregate(TimeSpan.Zero, (total, entry) => total + entry.TimeSpan.Value);

                if (bonus)
                {
                    remainingTime += TimeSpan.FromMinutes(30);
                }
                if (penalties)
                {
                    remainingTime -= TimeSpan.FromMinutes(30);

                }
                remainingTime = remainingTime - totalElapsedTime;
            }
            return new WeeklyModifiers
            {
                HasTimeBonuses = bonus,
                HasPenalties = penalties,
                WeeklyElapsedTime = totalElapsedTime,
                WeeklyRemainingTime = remainingTime,
            };
        }
    }
}
