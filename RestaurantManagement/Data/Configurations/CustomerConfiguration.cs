using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Data.Entities;

namespace RestaurantManagement.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("CUSTOMER");
            builder.HasKey(customer => customer.Id);
            builder.Property(customer => customer.FullName).IsRequired().IsUnicode().HasMaxLength(200);
            builder.Property(customer => customer.Gender).IsRequired();
            builder.Property(customer => customer.Birthday).IsRequired();
            builder.Property(customer => customer.VIP).IsRequired();
        }
    }
}
