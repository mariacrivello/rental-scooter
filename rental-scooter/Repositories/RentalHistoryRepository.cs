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
            return await _dataContext.RentalHistoryEntries
                            .Where(f => f.UserIdentifier.Equals(userIdentifier))
                            .OrderByDescending(f => f.Id)
                            .Include(f => f.Scooter)
                            .ToListAsync();
        }

        public async Task<List<RentalHistoryEntry>> GetByUserIdentifierFilteredByDate(string userIdentifier, DateTime startDate, DateTime endDate)
        {
            return await _dataContext.RentalHistoryEntries
                            .Where(f => f.UserIdentifier == userIdentifier &&
                                        f.RentalStartDateTime >= startDate && f.RentalStartDateTime <= endDate)
                            .OrderByDescending(f => f.Id)
                            .Include(f => f.Scooter)
                            .ToListAsync();
        }


        public async Task RentScooter(RentalHistoryEntry rentalHistoryEntry)
        {
            await _dataContext.RentalHistoryEntries.AddAsync(rentalHistoryEntry);
            await _dataContext.SaveChangesAsync();
        }

        public async Task ReturnScooter(RentalHistoryEntry rentalHistoryEntry)
        {
            _dataContext.Attach(rentalHistoryEntry);

            _dataContext.Entry(rentalHistoryEntry).Property(s => s.DropOffStationId).IsModified = true;
            _dataContext.Entry(rentalHistoryEntry).Property(s => s.RentalEndDateTime).IsModified = true;
            _dataContext.Entry(rentalHistoryEntry).Property(s => s.DurationAsTicks).IsModified = true;

            // Save changes to the database
            await _dataContext.SaveChangesAsync();
        }
    }
}
