using rental_scooter.Models;

namespace rental_scooter.Dtos
{
    public class UserHistoryEntryDto 
    {
        public List<RentalHistoryEntry>? RentalHistoryEntries { get; set; }
        public bool HasPenalties { get; set; }
        public bool HasTimeBonuses { get; set; }
        public TimeSpan? RemainingTime { get; set; }
    }

    public class UserRentDto
    {
        public int? ScootersRented { get; set; }
        public TimeSpan? TotalElapsedTime { get; set; }
        public bool HasOngoingRides { get; set; }
    }
}
