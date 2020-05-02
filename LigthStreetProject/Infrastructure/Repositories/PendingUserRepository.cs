using AutoMapper;
using Domain.Models;
using Domain.Root;
using Infrastructure.Models;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PendingUserRepository: Repository<PendingUserEntity, int>, IPendingUserRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<PendingUserEntity> _userManager;

        public PendingUserRepository(DbContext databaseContext,
            IMapper mapper,
            UserManager<PendingUserEntity> userManager)
            :base(databaseContext)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await databaseContext.Set<PendingUserEntity>().Where(x => x.NormalizedEmail == email.ToUpper()).FirstOrDefaultAsync() != null;
        }

        public async Task<PendingUser> RegisterAsync(PendingUser pendingUser, string password)
        {
            var entityUser = _mapper.Map<PendingUserEntity>(pendingUser);
            var result = await _userManager.CreateAsync(entityUser, password);
            return _mapper.Map<PendingUser>(entityUser);
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await databaseContext.Set<PendingUserEntity>().Where(x => x.NormalizedUserName == username.ToUpper()).FirstOrDefaultAsync() != null;
        }
    }
}
