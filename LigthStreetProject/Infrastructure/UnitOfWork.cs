using AutoMapper;
using Domain.Root;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork: IUnitOfWork
    {
        private IPointRepository pointRepository;

        private IPendingUserRepository pendingUserRepository;

        private readonly IMapper _mapper;

        private readonly UserManager<PendingUserEntity> _userManager;

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

        public IPendingUserRepository PendingUserRepository
        {
            get
            {
                if (pendingUserRepository == null)
                {
                    pendingUserRepository = new PendingUserRepository(DatabaseContext, _mapper, _userManager);
                }
                return pendingUserRepository;
            }
        }
        public DbContext DatabaseContext { get; private set; }

        public UnitOfWork(DbContext dbContext, IMapper mapper, UserManager<PendingUserEntity> userManager)
        {
            DatabaseContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
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
