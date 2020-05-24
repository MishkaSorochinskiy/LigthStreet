using AutoMapper;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LigthStreet.WebApi.Identity.Services.Interfaces
{
    public class ClientService : IClientService
    {
        private readonly IMapper _mapper;
        private readonly ConfigurationDbContext _configurationDbContext;

        public ClientService(ConfigurationDbContext configurationDbContext, IMapper mapper)
        {
            _configurationDbContext = configurationDbContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(string serialNumber, string password)
        {
            var client = new Client
            {
                ClientId = serialNumber,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret(password.Sha256())
                },
                AllowedScopes = { "LightStreet.WebApi" }
            };
            _configurationDbContext.Clients.Add(client.ToEntity());
            await _configurationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task Delete(string clientId)
        {
            var entityToRemove = await _configurationDbContext.Clients
                .Where(x => x.ClientId == clientId)
                .FirstOrDefaultAsync();
            if (entityToRemove != null)
            {
                _configurationDbContext.Clients.Remove(entityToRemove);
                await _configurationDbContext.SaveChangesAsync();
            }
        }
    }
}
