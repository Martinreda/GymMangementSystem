using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GymManagementDAL.Data.Contexts
{
    public class GymDbContextFactory : IDesignTimeDbContextFactory<GymDbContext>
    {
        public GymDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GymDbContext>();

            optionsBuilder.UseSqlServer("Server=.;Database=GymMangementDB02;Trusted_Connection=True;TrustServerCertificate=True");

            return new GymDbContext(optionsBuilder.Options);
        }
    }
}
