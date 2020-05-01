using Domain.Models;
using Domain.Root;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IPointRepository:IRepository<PointEntity, int>
    {
        Task<bool> Exists(int pointId);

        Task<PointEntity> GetByCoords(double lat, double lng);

        Task<List<PointEntity>> GetFromZone(double west, double east, double north, double south);
    }
}

