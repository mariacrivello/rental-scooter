
using Microsoft.EntityFrameworkCore;
using rental_scooter.Models;

namespace rental_scooter.Repositories
{
    public class ScooterRepository : IScooterRepository
    {
        private AppDbContext _dataContext;
        public ScooterRepository(AppDbContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<bool> DoesScooterExist(int scooterId)
        {
            return await _dataContext.Scooters
                            .AnyAsync(f => f.Id == scooterId);
        }
        public async Task<Scooter?> GetById(int scooterId)
        {
            return await _dataContext.Scooters
                            .FirstOrDefaultAsync(f => f.Id == scooterId);
        }
        public async Task UpdateScooter(Scooter scooter)
        {
            _dataContext.Attach(scooter);

            _dataContext.Entry(scooter).Property(s => s.State).IsModified = true;
            _dataContext.Entry(scooter).Property(s => s.StationId).IsModified = true;

            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateScooterOnReturn(Scooter scooter)
        {
            _dataContext.Attach(scooter);

            _dataContext.Entry(scooter).Property(s => s.State).IsModified = true;
            _dataContext.Entry(scooter).Property(s => s.StationId).IsModified = true;

            await _dataContext.SaveChangesAsync();
        }
    }
}
