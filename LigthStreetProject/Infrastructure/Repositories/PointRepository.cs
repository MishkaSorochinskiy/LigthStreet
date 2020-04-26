using Domain.Models;
using Domain.Root.Interrfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PointRepository : Repository<Point, int>, IPointRepository
    {
        public PointRepository(DbContext databaseContext)
            : base(databaseContext)
        {

        }

        public async Task<bool> Exists(int pointId)
        {
            return await databaseContext.Set<Point>().Where(x => x.Id == pointId).FirstOrDefaultAsync() != null;
        }

        public Task<List<Point>> GetFromZone(double west,double east,double north,double south)
        {
            return databaseContext.Set<Point>()
                .Where(p => p.Latitude >= west && p.Latitude <= east)
                .Where(p => p.Longtitude >= north && p.Longtitude <= south)
                .ToListAsync();
        }

        public Task<Point> GetByCoords(double lat,double lng)
        {
            return databaseContext.Set<Point>()
                .Where(p => p.Latitude == lat && p.Longtitude == lng)
                .FirstOrDefaultAsync();
        }
    }
}
