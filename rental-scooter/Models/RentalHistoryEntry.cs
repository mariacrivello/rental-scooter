namespace rental_scooter.Models
{
    public class RentalHistoryEntry
    {
        public int Id { get; set; }
        public required string UserIdentifier { get; set; }

        public int ScooterId { get; set; }
        public virtual Scooter Scooter { get; set; }

        public int PickUpStationId { get; set; }
        public virtual Station PickUpStation { get; set; }

        public int? DropOffStationId { get; set; }
        public virtual Station? DropOffStation { get; set; }

        public DateTime RentalStartDateTime { get; set; }
        public DateTime? RentalEndDateTime { get; set; }
        public TimeSpan? TimeSpan { get; set; }
    }
}
