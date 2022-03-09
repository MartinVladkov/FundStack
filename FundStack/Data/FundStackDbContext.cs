using FundStack.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FundStack.Data
{
    public class FundStackDbContext : IdentityDbContext
    {
        public DbSet<Asset> Assets { get; set; }

        public DbSet<Models.Type> Types { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Asset>()
                .HasOne(a => a.Portfolio)
                .WithMany(a => a.Assets)
                .HasForeignKey(a => a.PortfolioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Asset>()
                .HasOne(a => a.Type)
                .WithMany(a => a.Assets)
                .HasForeignKey(a => a.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasOne(p => p.Portfolio)
                .WithOne(p => p.User)
                .HasForeignKey<User>(u => u.PortfolioId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        public FundStackDbContext(DbContextOptions<FundStackDbContext> options)
            : base(options)
        {
        }
    }
}