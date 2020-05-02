using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigthStreet.WebApi.Identity.Services.Interfaces
{
    public interface IClientService
    {
        Task<bool> CreateAsync(string deviceId, string password);

        Task Delete(string clientId);
    }
}
