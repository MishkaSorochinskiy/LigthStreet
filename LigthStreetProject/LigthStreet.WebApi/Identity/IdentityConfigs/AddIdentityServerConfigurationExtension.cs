using IdentityServer4.AccessTokenValidation;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Infrastructure;
using Infrastructure.Models;
using LightStreet.Utilities;
using LigthStreet.WebApi.Data;
using LigthStreet.WebApi.Identity.Token;
using LigthStreet.WebApi.Identity.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace LigthStreet.WebApi.Identity.IdentityConfigs
{
    public static class AddIdentityServerConfigurationExtension
    {
        /// <summary>
        /// Extension method for configuring identity server.
        /// </summary>
        /// <param name="serviceCollection">Collection of services for registering custom services.</param>
        public static void AddIdentityServerConfiguration(this IServiceCollection serviceCollection,
            string connectionString)
        {
            serviceCollection.AddTransient<ITokenProvider, TokenProvider>();
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            serviceCollection.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                //.AddClientConfigurationValidator<ClientConfigurationValidator>()
                .AddAspNetIdentity<UserEntity>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                }).AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                });//.AddProfileService<ProfileService>();
        }

        /// <summary>
        /// Extension method for configuring identity server.
        /// </summary>
        /// <param name="serviceCollection">Collection of services for registering custom services.</param>
        public static void AddIdentityServerThirdPartyValidationConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Environment.GetEnvironmentVariable(EnvironmentConstants.JWT_AUTHORITY);
                options.RequireHttpsMetadata = false;
                options.Audience = Environment.GetEnvironmentVariable(EnvironmentConstants.JWT_AUDIENCE);
            });
        }

        public static void InitializeDatabaseForIdentitySever(this IServiceCollection app)
        {
            using (var serviceScope = app.BuildServiceProvider()/*var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()*/)
            {
                var agentDbContext = serviceScope.GetRequiredService<LightStreetContext>();
                agentDbContext.Database.Migrate();
                var persistedGrantDbContext = serviceScope
                    .GetRequiredService<PersistedGrantDbContext>();
                persistedGrantDbContext.Database.Migrate();
                var context = serviceScope.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityServerConfig.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityServerConfig.GetClients().ToList())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityServerConfig.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!agentDbContext.Users.Any(x => x.UserName == "Admin"))
                {
                    SeedData.Initialize(agentDbContext,
                        serviceScope.GetRequiredService<UserManager<UserEntity>>(),
                        serviceScope.GetRequiredService<RoleManager<RoleEntity>>());
                }
            }
        }
    }
}
