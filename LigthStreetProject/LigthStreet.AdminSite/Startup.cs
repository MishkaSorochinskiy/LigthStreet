using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LigthStreet.AdminSite.Data;
using LigthStreet.AdminSite.Services.Interfaces;
using LigthStreet.AdminSite.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace LigthStreet.AdminSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region Authorization

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("AllowGrafana", policy => { policy.RequireClaim("Permission", "AllowGrafana"); });
                opts.AddPolicy("AllowAdmin", policy => { policy.RequireClaim("Permission", "AllowAdmin"); });
                opts.AddPolicy("AllowUserManagement",
                    policy => { policy.RequireClaim("Permission", "AllowUserManagement"); });
                opts.AddPolicy("AllowAgentManagement",
                    policy => { policy.RequireClaim("Permission", "AllowAgentManagement"); });
                opts.AddPolicy("AllowAgentInstall",
                    policy => { policy.RequireClaim("Permission", "AllowAgentInstall"); });
                opts.AddPolicy("AllowFirmwareInstall",
                    policy => { policy.RequireClaim("Permission", "AllowFirmwareInstall"); });
            });

            #endregion

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddScoped<IAuthService, AuthService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
