using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data.Configurations;
using RestaurantManagement.Data.Entities;

namespace RestaurantManagement.Data
{
    public class RestaurantDbContext : IdentityDbContext<Customer, IdentityRole<Guid>, Guid>
    {
        public RestaurantDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TableConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new FoodConfiguration());
            modelBuilder.ApplyConfiguration(new OrderTableConfiguration());
            modelBuilder.ApplyConfiguration(new BillConfiguration());
            modelBuilder.ApplyConfiguration(new BillDetailConfiguration());
        }

        public DbSet<Table> Table { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<OrderTable> OderTable { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<BillDetail> BillDetail { get; set; }
    }
}