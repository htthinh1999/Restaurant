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
            modelBuilder.ApplyConfiguration(new Order_tableConfiguration());
        }

        public DbSet<Table> Table { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Order_table> Oder_table { get; set; }
    }
}
