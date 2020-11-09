using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Managers;
using HandiworkShop.DAL.Context;
using HandiworkShop.DAL.Entities;
using HandiworkShop.BLL.Repository;

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
            services.AddScoped<IRepository<Task>, Repository<Task>>();
            services.AddScoped<IRepository<Tag>, Repository<Tag>>();
            services.AddScoped<IRepository<Profile>, Repository<Profile>>();
            services.AddScoped<IRepository<Comment>, Repository<Comment>>();
            services.AddScoped<IRepository<UserTag>, Repository<UserTag>>();

            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IProfileManager, ProfileManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<ITaskManager, TaskManager>();
            services.AddScoped<ITagManager, TagManager>();
            services.AddScoped<ICommentManager, CommentManager>();


            string connectionString = Configuration.GetConnectionString("HandiworkShopApp");
            services.AddDbContext<HandiworkShopContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<HandiworkShopContext>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();


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
            });
        }
    }
}
