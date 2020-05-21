using Domain.Enums;
using Domain.Models;
using Domain.Root;
using Infrastructure.Models;
using Infrastructure.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository: IRepository<UserEntity, int>
    {
        Task<User> RegisterByPasswordHashAsync(User user, string passwordHash);

        Task ChangeUserRoleAsync(int userId, int roleId);

        Task<bool> UserExists(int userId);

        Task<IEnumerable<User>> GetPageAsync(int count, int page, string searchQuery, UserStatusTypeEntity userStatusType);

        Task ChangeStatusUserAsync(int userId, UserStatusType status);

        Task<IEnumerable<User>> GetAllActiveUsers();
    }
}
