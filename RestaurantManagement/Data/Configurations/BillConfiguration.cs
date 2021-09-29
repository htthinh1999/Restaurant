using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Data.Configurations
{
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable("BILL");
            builder.HasKey(bill => bill.Id);
            builder.Property(bill => bill.CustomerId).IsRequired();
            builder.Property(bill => bill.Total).IsRequired();
            builder.Property(bill => bill.PaymentMethod).IsRequired();
            builder.Property(bill => bill.CreatedDate).IsRequired();
            builder.HasOne(c => c.Customer)
                .WithMany(c => c.Bills)
                .HasForeignKey(c => c.CustomerId);
        }
    }
}
