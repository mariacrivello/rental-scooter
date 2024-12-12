using rental_scooter.Dtos;
using rental_scooter.Models;
using rental_scooter.Repositories;

namespace rental_scooter.Services
{
    public class RentalService : IRentalService
    {
        private readonly IStationRepository stationRepository;
        private readonly IRentalHistoryRepository rentalHistoryRepository;
        private readonly IScooterRepository scooterRepository;

        public RentalService(IStationRepository stationRepository, IRentalHistoryRepository rentalHistoryRepository, IScooterRepository scooterRepository)
        {
            this.stationRepository = stationRepository;
            this.rentalHistoryRepository = rentalHistoryRepository;
            this.scooterRepository = scooterRepository;
        }

        public async Task<UserHistoryEntryDto> GetHistoryEntriesByUserIdentifier(string user)
        {
            var historyEntries = await rentalHistoryRepository.GetByUserIdentifier(user);
            var modifications = new WeeklyModifiers();

            if (historyEntries != null)
            {
                modifications = GetModifications(historyEntries);
            }

            return new UserHistoryEntryDto
            {
                RentalHistoryEntries = historyEntries,
                HasPenalties = modifications.HasPenalties,
                HasTimeBonuses = modifications.HasTimeBonuses,
                RemainingTime = modifications.WeeklyRemainingTime
            };
        }

        public async Task<IEnumerable<Station>> GetStationsWithAvailableScooters()
        {
            var result = await stationRepository.GetStationsWithAvailableScootersAsync();
            return result;
        }

        public async Task RentScooter(ScooterRentRequest request)
        {
            var scooter = await ValidateDataOnRent(request);

            var rentObject = new RentalHistoryEntry
            {
                UserIdentifier = request.UserIdentifier,
                ScooterId = request.ScooterId,
                PickUpStationId = scooter.StationId.Value,
                RentalStartDateTime = DateTime.Now,
            };

            await rentalHistoryRepository.RentScooter(rentObject);

            scooter.State = State.Busy;
            scooter.StationId = null;

            await scooterRepository.UpdateScooter(scooter);
        }



        public async Task ReturnScooter(ScooterReturnRequest request)
        {
            (long timeElapsed, RentalHistoryEntry RentalHistoryEntry) = await ValidateDataOnReturn(request);

            var returnObject = RentalHistoryEntry;
            returnObject.DropOffStationId = request.StationId;
            returnObject.RentalEndDateTime = DateTime.Now;
            returnObject.DurationAsTicks = timeElapsed;

            await rentalHistoryRepository.ReturnScooter(returnObject);

            var scooterToReturn = returnObject.Scooter;

            scooterToReturn.State = State.Available;
            scooterToReturn.StationId = request.StationId;

            await scooterRepository.UpdateScooterOnReturn(scooterToReturn);
        }

        private async Task<Scooter> ValidateDataOnRent(ScooterRentRequest request)
        {
            var scooter = await scooterRepository.GetById(request.ScooterId);
            if (scooter is null)
            {
                throw new InvalidOperationException(message: "No se encontró el monopatín");
            }
            else if (scooter.State != State.Available)
            {
                throw new InvalidOperationException(message: "El monopatín seleccionado no está disponible");
            }
            else if (scooter.StationId is null)
            {
                throw new InvalidOperationException(message: "El monopatín seleccionado no está disponible");
            }

            var userRelatedHistoryEntries = await rentalHistoryRepository.GetByUserIdentifier(request.UserIdentifier);
            if (userRelatedHistoryEntries != null && userRelatedHistoryEntries.Count != 0)
            {
                var modifications = GetModifications(userRelatedHistoryEntries);
                if (userRelatedHistoryEntries.First().DropOffStationId is null)
                {
                    throw new InvalidOperationException(message: "el usuario tiene un viaje en curso");
                }

                else if (modifications.WeeklyRemainingTime <= TimeSpan.FromHours(0))
                {
                    throw new InvalidOperationException(message: "El usuario no posee tiempo restante");
                }
            }
            return scooter;
        }

        private async Task<(long, RentalHistoryEntry)> ValidateDataOnReturn(ScooterReturnRequest request)
        {
            var station = await stationRepository.GetByIdWithScooters(request.StationId);
            if (station is null)
            {
                throw new InvalidOperationException(message: "No se encontró la estación");
            }
            else if (station.Scooters?.Count >= 10)
            {
                throw new InvalidOperationException(message: "La estación no posee espacios disponibles para dejar el monopatín");
            }

            List<RentalHistoryEntry> userRelatedHistoryEntries = await rentalHistoryRepository.GetByUserIdentifier(request.UserIdentifier);
            if (!(userRelatedHistoryEntries != null && userRelatedHistoryEntries.Count != 0))
            {
                throw new InvalidOperationException(message: "El usuario no tiene viajes activos");
            }

            var mostRecentRent = userRelatedHistoryEntries.FirstOrDefault();
            if (mostRecentRent != null && mostRecentRent.DropOffStation != null)
            {
                throw new InvalidOperationException(message: "El usuario no tiene viajes activos");
            }

            var elapsedTime = (DateTime.Now - mostRecentRent.RentalStartDateTime).Ticks;
            return (elapsedTime, mostRecentRent);
        }

        private WeeklyModifiers GetModifications(List<RentalHistoryEntry> entries)
        {

            var lastWeekEntries = entries.Where(f => f.RentalStartDateTime >= DateTime.UtcNow.AddDays(-7));
            bool penalties = false;
            bool bonus = false;
            TimeSpan totalElapsedTime = TimeSpan.Zero;
            TimeSpan remainingTime = TimeSpan.FromHours(2);

            if (lastWeekEntries != null)
            {
                penalties = lastWeekEntries.Any(f => f.RentalDuration > TimeSpan.FromHours(2));
                bonus = lastWeekEntries.Count() > 2;

                foreach (var entry in lastWeekEntries)
                {
                    if (entry.RentalDuration != null)
                    {
                        totalElapsedTime += entry.RentalDuration.Value;
                    }
                }

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
                WeeklyElapsedTime = totalElapsedTime.Ticks,
                WeeklyRemainingTime = remainingTime,
            };
        }
    }
}
