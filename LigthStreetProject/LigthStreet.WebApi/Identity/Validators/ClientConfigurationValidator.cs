using IdentityServer4.Validation;
using LigthStreet.WebApi.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LigthStreet.WebApi.Identity.Validators
{
    public class ClientConfigurationValidator : IClientConfigurationValidator
    {
        private readonly IClientService _agentService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientConfigurationValidator(IClientService agentService, IHttpContextAccessor httpContextAccessor)
        {
            _agentService = agentService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ValidateAsync(ClientConfigurationValidationContext context)
        {
        }
    }
}
