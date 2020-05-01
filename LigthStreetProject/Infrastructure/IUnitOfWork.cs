using Infrastructure.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IPointRepository PointRepository { get; }
        Task Commit();
    }
}
