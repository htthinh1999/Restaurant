using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Constants;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement
{
    public class Startup
    {
        public static readonly ILoggerFactory SQLServerLoggerFactory =
        LoggerFactory.Create(
            builder =>
            {
                builder.AddConsole()
                       .AddFilter(level => level == LogLevel.Information);
            }
        );

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddDbContext<RestaurantDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString(SystemConstants.OnlineConnection))
                            .UseLoggerFactory(SQLServerLoggerFactory)
                            .EnableSensitiveDataLogging());

            // Add Identity
            services.AddIdentity<Customer, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<RestaurantDbContext>()
                .AddDefaultTokenProviders();

            // Register Dependence Injection (DI)
            services.AddTransient<SignInManager<Customer>, SignInManager<Customer>>();
            services.AddTransient<ICustomerService, CustomerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
