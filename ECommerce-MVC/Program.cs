using ECommerce_MVC.Models.Data;
using ECommerce_MVC.Models.Model;
using ECommerce_MVC.Models.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("con1");
            builder.Services.AddDbContext<ECommerceContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddIdentity<AppUser, IdentityRole>(r =>
            {
                r.Password.RequiredLength = 6;
                r.Password.RequireDigit = false;
                r.Password.RequireLowercase = false;
                r.Password.RequireNonAlphanumeric = false;
                r.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ECommerceContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
