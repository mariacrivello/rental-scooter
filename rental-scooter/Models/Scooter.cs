using static System.Collections.Specialized.BitVector32;

namespace rental_scooter.Models
{
    public enum State
    {
        Available, Busy, NotAvailable
    }
    public class Scooter
    {
        public int Id { get; set; }
        public State State { get; set; }
        public int? StationId { get; set; }
        public virtual Station? Station { get; set; }
    }
}
