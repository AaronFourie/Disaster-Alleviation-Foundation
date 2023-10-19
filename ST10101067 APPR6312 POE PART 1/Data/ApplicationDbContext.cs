using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_1.Models;

namespace ST10101067_APPR6312_POE_PART_1
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
        public DbSet<ST10101067_APPR6312_POE_PART_1.Models.Admin> Admin { get; set; } = default!;
    }
}