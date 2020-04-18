using Domain.Models;
using Domain.Root;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Root.Interrfaces
{
    public interface IPointRepository:IRepository<Point, int>
    {

        bool Exists(int pointId);
    }
}

