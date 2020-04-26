using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Root.Interrfaces
{
    public interface IPointRepository:IRepository<Point, int>
    {
        Task<bool> Exists(int pointId);

        Task<Point> GetByCoords(double lat, double lng);

        Task<List<Point>> GetFromZone(double west, double east, double north, double south);
    }
}

