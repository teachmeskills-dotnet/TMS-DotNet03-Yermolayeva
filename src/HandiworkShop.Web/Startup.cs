using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Managers;
using HandiworkShop.BLL.Repository;
using HandiworkShop.DAL.Context;
using HandiworkShop.DAL.Entities;
using HandiworkShop.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;

namespace HandiworkShop.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRepository<Order>, Repository<Order>>();
            services.AddScoped<IRepository<DAL.Entities.Task>, Repository<DAL.Entities.Task>>();
            services.AddScoped<IRepository<Tag>, Repository<Tag>>();
            services.AddScoped<IRepository<Profile>, Repository<Profile>>();
            services.AddScoped<IRepository<UserTag>, Repository<UserTag>>();
            services.AddScoped<IRepository<OrderTag>, Repository<OrderTag>>();

            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IProfileManager, ProfileManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<ITaskManager, TaskManager>();
            services.AddScoped<ITagManager, TagManager>();

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            string connectionString = Configuration.GetConnectionString("HandiworkShopApp");
            services.AddDbContext<HandiworkShopContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<HandiworkShopContext>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            //app.UseDeveloperExceptionPage();
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            CreateRoles(serviceProvider);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task<IdentityResult> roleResult;

            Task<bool> hasVendorRole = roleManager.RoleExistsAsync("Vendor");
            hasVendorRole.Wait();

            if (!hasVendorRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole("Vendor"));
                roleResult.Wait();
            }
        }
    }
}