using Microsoft.EntityFrameworkCore;
using rental_scooter.Models;

namespace rental_scooter.Repositories
{
    public class RentalHistoryRepository : IRentalHistoryRepository
    {
        private AppDbContext _dataContext;
        public RentalHistoryRepository(AppDbContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<List<RentalHistoryEntry>> GetByUserIdentifier(string userIdentifier)
        {
            return await _dataContext.RentalHistoryEntries.Where(f => f.UserIdentifier.Equals(userIdentifier)).ToListAsync();
        }

        public Task RentScooter(string userId, int scooterId)
        {
            throw new NotImplementedException();
        }

        public Task RentScooter(RentalHistoryEntry rentalHistoryEntry)
        {
            throw new NotImplementedException();
        }

        public Task ReturnScooter(string userId, int stationId)
        {
            throw new NotImplementedException();
        }
    }
}
