namespace rental_scooter.Dtos
{
    public class ScooterRentRequest
    {
        public required string UserIdentifier { get; set; }
        public int ScooterId { get; set; }
    }

    public class ScooterReturnRequest
    {
        public required string UserIdentifier { get; set; }
        public int StationId { get; set; }
    }

}
