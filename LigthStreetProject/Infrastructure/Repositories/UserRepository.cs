using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Models;
using Infrastructure.Models.Enums;
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
    public class UserRepository : Repository<UserEntity, int>, IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<UserEntity> _userManager;

        public UserRepository(DbContext databaseContext,
            IMapper mapper,
            UserManager<UserEntity> userManager)
            : base(databaseContext)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task ChangeStatusUserAsync(int userId, UserStatusType status)
        {
            var userStatus = _mapper.Map<UserStatusTypeEntity>(status);
            var userForUpdate = await databaseContext.Set<UserEntity>().Where(x => x.Id == userId).SingleOrDefaultAsync();
            userForUpdate.Status = userStatus;
            await databaseContext.SaveChangesAsync();
        }

        public async Task ChangeUserRoleAsync(int userId, int roleId)
        {
            var userEntity = await databaseContext.Set<UserEntity>().FirstOrDefaultAsync(x => x.Id == userId);
            if (userEntity == null) { throw new Exception("User not found"); }
            var roleEntity = await databaseContext.Set<RoleEntity>().FirstOrDefaultAsync(x => x.Id == roleId);
            if (roleEntity == null) { throw new Exception("Role not found"); }
            if (userEntity.UserName == "Admin") { throw new Exception("The role for main Admin cannot be changed"); }

            var allUserRoles = await _userManager.GetRolesAsync(userEntity);
            var deleteAllUserRolesResult = await _userManager.RemoveFromRolesAsync(userEntity, allUserRoles);
            if (!deleteAllUserRolesResult.Succeeded)
            {
                throw new Exception(deleteAllUserRolesResult.Errors.Select(x => x.Description).ToString());
            }
           
            var result = await _userManager.AddToRoleAsync(userEntity, roleEntity.Name);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(x => x.Description).ToString());
            }
        }

        public async Task<IEnumerable<User>> GetAllActiveUsers()
        {
            return _mapper.Map<List<User>>(await databaseContext.Set<UserEntity>().Where(x => x.Status == UserStatusTypeEntity.Active && x.IsDeleted == false).ToListAsync());
        }

        public async Task<IEnumerable<User>> GetPageAsync(int count, int page, string searchQuery, UserStatusTypeEntity userStatusType)
        {
            var query = databaseContext.Set<UserEntity>().Where(x=>x.Status == userStatusType && x.IsDeleted == false).AsQueryable();
            if (searchQuery != null)
            {
                query = query.Where(x => x.UserName.ToUpper().Contains(searchQuery.ToUpper())
                || x.LastName.ToUpper().Contains(searchQuery.ToUpper()) || x.FirstName.ToUpper().Contains(searchQuery.ToUpper()));
            }
            query = query.Skip(page * count).Take(count);
            return _mapper.Map<List<User>>(await query.ToListAsync());
        }

        public async Task<User> RegisterByPasswordHashAsync(User user, string passwordHash)
        {
            var entityUser = _mapper.Map<UserEntity>(user);
            var result = await _userManager.CreateAsync(entityUser);
            if (result.Succeeded)
            {
                entityUser.PasswordHash = passwordHash;
                databaseContext.Attach(entityUser);
                await databaseContext.SaveChangesAsync();
                return _mapper.Map<User>(entityUser);
            }
            throw new Exception("Cannot register user");
        }

        public async Task<bool> UserExists(int userId)
        {
            return await databaseContext.Set<UserEntity>().AsNoTracking().AnyAsync(x => x.Id == userId);
        }
    }
}
