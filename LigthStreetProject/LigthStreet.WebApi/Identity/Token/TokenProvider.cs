using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using LigthStreet.WebApi.Identity.IdentityConfigs;
using System.Threading.Tasks;


namespace LigthStreet.WebApi.Identity.Token
{
    public class TokenProvider : ITokenProvider
    {
        private readonly ITokenService _tokenService;
        private readonly IdentityServerOptions _options;

        public TokenProvider(ITokenService tokenService,
            IdentityServerOptions options)
        {
            _tokenService = tokenService;
            _options = options;
        }

        public async Task<string> GetToken(string subjectId, string deviceId, string secret)
        {
            var request = new TokenCreationRequest();
            var identityUser = new IdentityServerUser(subjectId);
            identityUser.DisplayName = deviceId;
            identityUser.AuthenticationTime = System.DateTime.UtcNow;
            identityUser.IdentityProvider = IdentityServerConstants.LocalIdentityProvider;
            request.Subject = identityUser.CreatePrincipal();
            request.IncludeAllIdentityClaims = true;
            request.ValidatedRequest = new ValidatedRequest();
            request.ValidatedRequest.Subject = request.Subject;
            request.ValidatedRequest.SetClient(new Client()
            {
                ClientId = deviceId,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret(secret.Sha256())
                }
            });
            request.Resources = new Resources(IdentityServerConfig.GetIdentityResources(),
                IdentityServerConfig.GetApiResources());
            request.ValidatedRequest.Options = _options;
            request.ValidatedRequest.ClientClaims = identityUser.AdditionalClaims;
            var token = await _tokenService.CreateAccessTokenAsync(request);
            var tokenValue = await _tokenService.CreateSecurityTokenAsync(token);
            return tokenValue;
        }
    }
}
