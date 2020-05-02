using AutoMapper;
using Domain.Models;
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
    }
}
