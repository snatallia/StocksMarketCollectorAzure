using API.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Contex
{
    public class StockContext: DbContext    
    {
        public StockContext(DbContextOptions options) : base(options) { }

        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
