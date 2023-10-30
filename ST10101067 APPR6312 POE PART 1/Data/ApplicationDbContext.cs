using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_2.Models;

namespace ST10101067_APPR6312_POE_PART_2
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Models.MoneyDonation> MoneyDonation { get; set; }
        public DbSet<Models.GoodsDonation> GoodsDonation { get; set; }
        public DbSet<Models.Disaster> Disatser { get; set; }
        public DbSet<Models.Money> Money { get; set; }
        public DbSet<MoneyAllocation> MoneyAllocation { get; set; }
        public DbSet<GoodsAllocation> GoodsAllocation { get; set; }
        public DbSet<GoodsPurchase> GoodsPurchase { get; set; }
        public DbSet<GoodsInventory> GoodsInventory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Disaster entity to use the date data type
            modelBuilder.Entity<Disaster>()
                .Property(d => d.STARTDATE)
                .HasColumnType("date");

            modelBuilder.Entity<Disaster>()
                .Property(d => d.ENDDATE)
                .HasColumnType("date");

            // Configure the Disaster entity to use the date data type
            modelBuilder.Entity<MoneyDonation>()
                .Property(d => d.DATE)
                .HasColumnType("date");

            modelBuilder.Entity<GoodsDonation>()
                .Property(d => d.DATE)
                .HasColumnType("date");
            // Define the relationship between MoneyAllocation and Disaster
            // Define the relationship between MoneyAllocation and Disaster

        }

       

    }
}