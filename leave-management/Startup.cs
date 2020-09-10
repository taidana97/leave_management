using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using leave_management.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using leave_management.Contracts;
using leave_management.Respository;
using AutoMapper;
using leave_management.Mappings;

namespace leave_management
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            // Add references for Respository and Contracts to Startup file
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>(); // *
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>(); // *
            services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>(); // *

            services.AddAutoMapper(typeof(Maps));

            // thay doi ls 29 Employee -> Employee
            services.AddDefaultIdentity<Employee>(options => 
            {
                //options.SignIn.RequireConfirmedAccount = true; // khong Confirmed Account

                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;                
            })
                .AddRoles<IdentityRole>() // Add role
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            UserManager<Employee> userManager, // sau buoc add role vao services
            RoleManager<IdentityRole> roleManager // sau buoc add role vao services
            )
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

            SeedData.Seed(userManager, roleManager); // nmoi tao class ls23
            // dung` de tao data table roles dbo.AspNetRoles
            // dung` de tao data table users dbo.AspNetUserRoles and dbo.AspNetUsers

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
