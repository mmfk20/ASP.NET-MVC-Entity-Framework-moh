using System.Data.Entity;

namespace InsuranceMVC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Insuree> Insurees { get; set; }
    }
}
