using Microsoft.EntityFrameworkCore;
using WebApi.DataAccess.Entities;

namespace WebApi.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts)
        {
            // NOTE: supports Visual Studio item scaffolder tool
        }

        public DbSet<User> Users { get; set; }
    }
}
