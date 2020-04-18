using Domain.Root.Interrfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Root
{
    public interface IUnitOfWork : IDisposable
    {
        IPointRepository PointRepository { get; }
        Task Commit();
    }
}
