using rental_scooter.Models;

namespace rental_scooter.Dtos
{
    public class UserHistoryEntryDto 
    {
        public List<RentalHistoryEntry> rentalHistoryEntries { get; set; }
        public bool HasPenalties { get; set; }
        public bool HasTimeBonuses { get; set; }
        public TimeSpan? RemainingTime { get; set; }
    }
}
