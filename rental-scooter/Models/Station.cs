namespace rental_scooter.Models
{
    public class Station
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Scooter>? Scooters { get; set; } = new List<Scooter>();

    }
}
