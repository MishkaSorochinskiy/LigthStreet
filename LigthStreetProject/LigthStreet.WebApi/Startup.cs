using Domain.AzureConnections;
using Domain.AzureConnections.Interfaces;
using Domain.Root;
using Domain.Root.Interrfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            #region Add AzureStorage dependencies
            services.AddTransient<IAzureStorageConnection, AzureStorageConnection>();
            #endregion
            #region Add Entity Framework and Identity Framework 
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LightStreetContext>(options =>
                options.UseSqlServer(connection));
            #endregion
            services.AddCors();

            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IImageHandlerService, ImageHandlerService>();
            services.AddScoped<DbContext, LightStreetContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPointRepository, PointRepository>();

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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
