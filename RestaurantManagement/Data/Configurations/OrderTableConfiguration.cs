using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Data.Entities;

namespace RestaurantManagement.Data.Configurations
{
    public class OrderTableConfiguration : IEntityTypeConfiguration<OrderTable>
    {
        public void Configure(EntityTypeBuilder<OrderTable> builder)
        {
            builder.ToTable("ODER_TABLE");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.From).IsRequired();
            builder.Property(t => t.To).IsRequired();
            builder.Property(t => t.CustomerId).IsRequired();
            builder.Property(t => t.TableId).IsRequired();


            builder.HasOne(c => c.Customer)
                .WithMany(c => c.OrderTables)
                .HasForeignKey(c => c.CustomerId);
            builder.HasOne(t => t.Table)
                .WithMany(t => t.OrderTables)
                .HasForeignKey(t => t.TableId);
        }
    }
}