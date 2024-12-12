namespace rental_scooter.Dtos
{
    public class WeeklyModifiers
    {
        public bool HasTimeBonuses { get; set; }
        public bool HasPenalties { get; set; }

        public long? WeeklyElapsedTime {  get; set; }

        public TimeSpan? WeeklyRemainingTime { get; set; } = TimeSpan.FromHours(2);
    }
}
