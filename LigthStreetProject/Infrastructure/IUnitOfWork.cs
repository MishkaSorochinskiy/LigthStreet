using Infrastructure.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IPendingUserRepository PendingUserRepository { get; }
        IPointRepository PointRepository { get; }
        Task Commit();
    }
}
