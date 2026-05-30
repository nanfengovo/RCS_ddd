using Microsoft.EntityFrameworkCore;
using RCS.Core.Modules.Wms.Entities;
using RCS.Core.Modules.Wms.Repositories;
using RCS.Infrastructure.Persistence.EntityFramework;

namespace RCS.Infrastructure.Modules.Wms.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly RcsDbContext _dbContext;

        public LocationRepository(RcsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Location?> GetByCodeAsync(string code)
        {
            return await _dbContext.Locations.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task UpdateAsync(Location location)
        {
            _dbContext.Locations.Update(location);
            await _dbContext.SaveChangesAsync();
        }
    }
}