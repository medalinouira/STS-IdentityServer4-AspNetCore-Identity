/// Mohamed Ali NOUIRA
/// http://www.mohamedalinouira.com
/// https://github.com/medalinouira
/// Copyright © Mohamed Ali NOUIRA. All rights reserved.

using System;
using STS.Data;
using STS.Models;
using STS.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace STS
{
    public class Startup
    {
        #region Fields
        public IConfiguration _iConfiguration { get; set; }
        private IWebHostEnvironment _iWebHostEnvironment { get; set; }
        #endregion

        #region Constructor
        public Startup(IConfiguration _iConfiguration, 
            IWebHostEnvironment _iWebHostEnvironment)
        {
            this._iConfiguration = _iConfiguration;
            this._iWebHostEnvironment = _iWebHostEnvironment;
        }
        #endregion

        #region Methods
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddCors(options =>
            {
                options.AddPolicy("STSCorsPolicy", corsBuilder =>
                {
                    corsBuilder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(origin => origin == "http://localhost:4200")
                    .AllowCredentials();
                });
            });

            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddTransient<IProfileService, STSProfileService>();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Authentication.CookieLifetime = TimeSpan.FromMinutes(15);
            })
                .AddInMemoryIdentityResources(Configs.GetIdentityResources())
                .AddInMemoryApiResources(Configs.GetApiResources())
                .AddInMemoryClients(Configs.GetClients())
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<STSProfileService>();


            if (_iWebHostEnvironment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("TODO : CONFIGURE YOUR KEY MATERIAL");
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_iWebHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCors("STSCorsPolicy");

            app.UseIdentityServer();
            app.UseRouting();

            app.UseMvcWithDefaultRoute();
        }
        #endregion
    }
}
