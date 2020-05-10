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

        private ITagRepository tagRepository;

        private IUserRepository userRepository;

        private readonly IMapper _mapper;

        private readonly UserManager<PendingUserEntity> _pendingUserManager;

        private readonly UserManager<UserEntity> _userManager;


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

        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(DatabaseContext, _mapper, _userManager);
                }
                return userRepository;
            }
        }

        public IPendingUserRepository PendingUserRepository
        {
            get
            {
                if (pendingUserRepository == null)
                {
                    pendingUserRepository = new PendingUserRepository(DatabaseContext, _mapper, _pendingUserManager, this.UserRepository);
                }
                return pendingUserRepository;
            }
        }
        public DbContext DatabaseContext { get; private set; }

        public ITagRepository TagRepository
        {
            get
            {
                if (pendingUserRepository == null)
                {
                    tagRepository = new TagRepository(DatabaseContext, _mapper);
                }
                return tagRepository;
            }
        }
        public UnitOfWork(LightStreetContext dbContext, IMapper mapper, 
            UserManager<PendingUserEntity> pendingUserManager,
            UserManager<UserEntity> userManager)
        {
            DatabaseContext = dbContext;
            _mapper = mapper;
            _pendingUserManager = pendingUserManager;
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
