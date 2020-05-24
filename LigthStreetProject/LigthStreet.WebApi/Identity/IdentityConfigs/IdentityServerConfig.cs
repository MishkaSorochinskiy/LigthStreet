using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using LightStreet.Utilities;

namespace LigthStreet.WebApi.Identity.IdentityConfigs
{
    public static class IdentityServerConfig
    {
        /// <summary>
        /// APIs allowed to access the Auth server
        /// </summary>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("LightStreet.WebApi", "Light Street WebApi",
                    new[] { JwtClaimTypes.Name, JwtClaimTypes.Role, JwtClaimTypes.Id }),
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "browser",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,
                    AllowedScopes = {Environment.GetEnvironmentVariable(EnvironmentConstants.JWT_AUDIENCE),
                        IdentityServerConstants.StandardScopes.OpenId},
                    AllowOfflineAccess = true
                }
            };
        }

        public static IdentityOptions GetIdentityOptions()
        {
            IdentityOptions identityOptions = new IdentityOptions();
            identityOptions.User.RequireUniqueEmail = false;
            identityOptions.Password.RequiredLength = 20;
            identityOptions.Password.RequiredUniqueChars = 5;
            identityOptions.Password.RequireDigit = true;
            identityOptions.Password.RequireLowercase = true;
            identityOptions.Password.RequireNonAlphanumeric = true;
            identityOptions.Password.RequireUppercase = true;
            return identityOptions;
        }
    }
}
