using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Repositories;
using Akalaat.DAL.Data;
using Akalaat.DAL.Models;
using Akalaat.Models;
using Akalaat.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Akalaat
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            builder.Services.AddScoped<FileManagement>();
            builder.Services.AddScoped<EmailManagement>();
            builder.Services.AddScoped<AccountRepository>();
            builder.Services.AddScoped(typeof(IDistrictRepository),typeof(DistrictRepository));
            builder.Services.AddScoped(typeof(IRegionRepository),typeof(RegionRepository));
            builder.Services.AddScoped(typeof(IBranchDeliveryRepository),typeof(BranchDeliveryRepository));
            builder.Services.AddDbContext<AkalaatDbContext>(op =>
                op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                //options.IdleTimeout = TimeSpan.FromDays(1);
                //options.Cookie.HttpOnly = true;
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
                AddEntityFrameworkStores<AkalaatDbContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication().AddMicrosoftAccount(Microsoftoptions =>
            {
                Microsoftoptions.ClientId = builder.Configuration["MicrosoftClientId"]!;
                Microsoftoptions.ClientSecret = builder.Configuration["MicrosoftSecretId"]!;
            });
            builder.Services.Configure<SMTPConfigModel>(builder.Configuration.GetSection("SMTPConfig"));



            var app = builder.Build();



            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var context = services.GetRequiredService<AkalaatDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await context.Database.MigrateAsync();
                await DataSeeding.SeedDataAsync(context, loggerFactory, userManager, roleManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex.Message, "An Error occured during applying migration");
            }



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Restaurant}/{action=Home}/{id?}");

            app.Run();
        }
    }
}
