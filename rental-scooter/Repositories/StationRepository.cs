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

      
        public async Task<bool> DoesStationExist(int stationId)
        {
            return await _dataContext.Stations.AnyAsync(f => f.Id == stationId);
        }

        public async Task<Station> GetByIdWithScooters(int stationId)
        {
            return await _dataContext.Stations
                .Where(f=> f.Id == stationId)
                .Include(f => f.Scooters)
                .FirstOrDefaultAsync();
        }

        public async Task<Station?> GetStationByScooterId(int? scooterId)
        {
            return await _dataContext.Stations.Where(f => f.Scooters.Any(g => g.Id == scooterId)).FirstOrDefaultAsync();
        }

        public async Task <IEnumerable<Station>>GetStationsWithAvailableScootersAsync()
        {
            return await _dataContext.Stations.Include(f=> f.Scooters).ToListAsync();
        }
    }
}
