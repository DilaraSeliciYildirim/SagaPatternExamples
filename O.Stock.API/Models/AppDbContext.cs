using Microsoft.EntityFrameworkCore;

namespace O.Stock.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
    }
}
