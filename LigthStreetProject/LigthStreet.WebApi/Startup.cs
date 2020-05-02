using Domain.AzureConnections;
using Domain.AzureConnections.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using LigthStreet.WebApi.Identity.IdentityConfigs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace LigthStreet.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddAuthorization(opts =>
                {
                    opts.AddPolicy("AllowLogView", policy => { policy.RequireClaim("Permission", "AllowLogView"); });
                    opts.AddPolicy("AllowAdmin", policy => { policy.RequireClaim("Permission", "AllowAdmin"); });
                    opts.AddPolicy("AllowUserManagement", policy => { policy.RequireClaim("Permission", "AllowUserManagement"); });
                    opts.AddPolicy("AllowAgentManagement", policy => { policy.RequireClaim("Permission", "AllowAgentManagement"); });
                    opts.AddPolicy("AllowAgentInstall", policy => { policy.RequireClaim("Permission", "AllowAgentInstall"); });
                    opts.AddPolicy("AllowFirmwareInstall", policy => { policy.RequireClaim("Permission", "AllowFirmwareInstall"); });
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            #region Add AzureStorage dependencies
            services.AddTransient<IAzureStorageConnection, AzureStorageConnection>();
            #endregion
            #region Add Entity Framework and Identity Framework 
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LightStreetContext>(options =>
                options.UseSqlServer(connection));
            #endregion

            services.AddControllers()
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
               });

            services.AddCors();
           
            services.AddTransient<IImageService, ImageService>();
            services.AddScoped<DbContext, LightStreetContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPointRepository, PointRepository>();
            services.AddScoped<IPendingUserRepository, PendingUserRepository>();

            services.AddIdentityConfiguration();

            services.AddIdentityServerThirdPartyValidationConfiguration();

            services.AddIdentityServerConfiguration(connection);
            services.InitializeDatabaseForIdentitySever();
            //services.AddTransient<IClientService, ClientService>();

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
               .AllowAnyHeader()
               .AllowAnyMethod()
               .SetIsOriginAllowed(_ => true)
               .AllowCredentials()
           );

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
