namespace rental_scooter.Dtos
{
    public class TimeModifiers
    {
        public bool HasTimeBonuses { get; set; }
        public bool HasPenalties { get; set; }

        public long? ElapsedTime {  get; set; }

        public TimeSpan? RemainingTime { get; set; } = TimeSpan.FromHours(2);
    }
}
