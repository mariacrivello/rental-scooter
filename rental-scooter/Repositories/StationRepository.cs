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

        public async Task <IEnumerable<Station>>GetStationsWithAvailableScootersAsync()
        {
            return await _dataContext.Stations.Include(f=> f.Scooters).ToListAsync();
        }
    }
}
