using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Repositiories.Classes;
using GymManagementDAL.Repositiories.Interfaces;
using GymManagmentBSL;
using GymManagmentBSL.Services.Classes;
using GymManagmentBSL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace GymMangementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
            );
           

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(X=> X.AddProfile(new MappingProfiles()));
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAnalyticsService , AnalyticsService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
           
         
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register your services
            builder.Services.AddScoped<IPlanService, PlanService>();


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register your services
            builder.Services.AddScoped<ITrainerService, TrainerService>();  

            // باقي التسجيلات...
            // builder.Services.AddScoped<IMemberService, MemberService>();

            var app = builder.Build();
            //builder.Services.AddScoped<ITrainerRepositories, TrainerRepository>();
            
            
          



            #region Migrate Database -- Data Seeding 
            var Scope = app.Services.CreateScope();
            var dbContext = Scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var PendingMigrations = dbContext.Database.GetPendingMigrations();
            if (PendingMigrations?.Any() ?? false)
                dbContext.Database.Migrate(); 
            GymDbContextSeeding.SeedData(dbContext);
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // ... possibly production-only error handling middleware ...
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapStaticAssets(); // Non-standard method, likely a custom helper or an older/renamed method

            // Custom Route 1: Trainers
            app.MapControllerRoute(
                name: "Trainers",
                pattern: "coach/{action}",
                defaults: new { controller = "Trainer" , action = "Index" });

            // Standard Default Route 
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            // WithStaticAssets() is non-standard/likely custom

   


            app.Run();
        }
    }
}
