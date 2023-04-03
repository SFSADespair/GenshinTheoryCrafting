using GenshinTheoryCrafting.Models;

namespace GenshinTheoryCrafting.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Users> Users => Set<Users>();
    }
}
