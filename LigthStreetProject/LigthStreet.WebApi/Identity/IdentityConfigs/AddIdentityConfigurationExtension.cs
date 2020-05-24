using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ManyToMany;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LigthStreet.WebApi.Identity.IdentityConfigs
{
    /// <summary>
    /// Extension class for adding identity configs.
    /// </summary>
    public static class AddIdentityConfigurationExtension
    {
        /// <summary>
        /// Extension method for configuring identity.
        /// </summary>
        /// <param name="serviceCollection">Collection of services for registering custom services.</param>
        public static void AddIdentityConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddIdentity<UserEntity, RoleEntity>(option => option = IdentityServerConfig.GetIdentityOptions())
                .AddEntityFrameworkStores<LightStreetContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<UserEntity, RoleEntity, LightStreetContext, int, IdentityUserClaim<int>,
                    UserRoleEntity, IdentityUserLogin<int>, IdentityUserToken<int>, IdentityRoleClaim<int>>>()
                .AddRoleStore<RoleStore<RoleEntity, LightStreetContext, int, UserRoleEntity, IdentityRoleClaim<int>>>();

            serviceCollection.AddIdentityCore<PendingUserEntity>(option => option = IdentityServerConfig.GetIdentityOptions())
                .AddEntityFrameworkStores<LightStreetContext>();
        }
    }
}
