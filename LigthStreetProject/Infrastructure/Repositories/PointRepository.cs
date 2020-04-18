using Domain.Models;
using Domain.Root.Interrfaces;
using Microsoft.EntityFrameworkCore;
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

        public bool Exists(int pointId)
        {
            return databaseContext.Set<Point>().Where(x => x.Id == pointId).FirstOrDefault() != null;
        }
    }
}
