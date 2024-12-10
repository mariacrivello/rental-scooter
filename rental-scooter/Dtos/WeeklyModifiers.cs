namespace rental_scooter.Dtos
{
    public class WeeklyModifiers
    {
        public bool HasTimeBonuses { get; set; }
        public bool HasPenalties { get; set; }

        public TimeSpan? WeeklyElapsedTime {  get; set; }

        public TimeSpan? WeeklyRemainingTime { get; set;}
    }
}
