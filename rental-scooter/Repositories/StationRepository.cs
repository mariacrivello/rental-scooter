using Microsoft.EntityFrameworkCore;
using rental_scooter.Models;

namespace rental_scooter.Repositories
{
    public class StationRepository : IStationRepository
    {
        private AppDbContext _dataContext;
        public StationRepository(AppDbContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public Task<bool> DoesScooterExist(int scooterId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoesStationExist(int stationId)
        {
            throw new NotImplementedException();
        }

        public async Task<Station?> GetByScooterId(int? scooterId)
        {
            return await _dataContext.Stations.Where(f => f.Scooters.Any(g => g.Id == scooterId)).FirstOrDefaultAsync();
        }

        public async Task <IEnumerable<Station>>GetStationsWithAvailableScootersAsync()
        {
            return await _dataContext.Stations.Include(f=> f.Scooters).ToListAsync();
        }
    }
}
