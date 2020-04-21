using Domain.Root;
using Domain.Root.Interrfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork: IUnitOfWork
    {
        private IPointRepository pointRepository;

        public IPointRepository PointRepository
        {
            get
            {
                if (pointRepository == null)
                {
                    pointRepository = new PointRepository(DatabaseContext);
                }
                return pointRepository;
            }
        }
        public DbContext DatabaseContext { get; private set; }

        public UnitOfWork(DbContext dbContext)
        {
            DatabaseContext = dbContext;
        }

        public Task Commit()
        {
            return DatabaseContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DatabaseContext.Dispose();
        }
    }
}
