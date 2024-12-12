using rental_scooter.Models;

namespace rental_scooter.Repositories
{
    public interface IScooterRepository
    {
        Task<bool> DoesScooterExist(int scooterId);
        Task<Scooter> GetById(int scooterId);
        Task UpdateScooter(Scooter scooter);
        Task UpdateScooterOnReturn (Scooter scooter);
    }
}
