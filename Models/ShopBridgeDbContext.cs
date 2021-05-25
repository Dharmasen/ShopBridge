using Microsoft.EntityFrameworkCore;
using ShopBridge.Models;
using System;

namespace Microsoft.EntityFrameworkCore
{
    public class ShopBridgeDbContext : DbContext
    {

        private string connectionStringValue;
        public ShopBridgeDbContext(string connectionString)
        {
            connectionStringValue = connectionString;
        }

        public ShopBridgeDbContext(DbContextOptions<ShopBridgeDbContext> options)
      : base(options)
        {
        }
        public DbSet<InventoryItems> InventoryItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionStringValue);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryItems>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("InventoryItems");
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(100);
                entity.Property(e => e.Price).HasMaxLength(100);
                entity.Property(e => e.Category).HasMaxLength(100);
            });
        }


    }
}
