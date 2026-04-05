using Microsoft.EntityFrameworkCore;

namespace SmartSpend.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
        {

        }

        public DbSet<Expense> Expenses { get; set; }
    }
}
