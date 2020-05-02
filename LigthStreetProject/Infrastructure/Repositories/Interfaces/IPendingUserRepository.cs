using Domain.Models;
using Domain.Root;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IPendingUserRepository: IRepository<PendingUserEntity, int>
    {
        Task<PendingUser> RegisterAsync(PendingUser pendingUser, string password);

        Task<bool> UsernameExists(string username);

        Task<bool> EmailExists(string email);
    }
}
