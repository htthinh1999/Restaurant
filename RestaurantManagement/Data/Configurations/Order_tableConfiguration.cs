using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Data.Entities;

namespace RestaurantManagement.Data.Configurations
{
    public class Order_tableConfiguration : IEntityTypeConfiguration<Order_table>
    {
        public void Configure(EntityTypeBuilder<Order_table> builder)
        {
            builder.ToTable("ODER_TABLE");
            builder.HasKey(t => t.ID);
            builder.HasOne(c => c.Customer)
                .WithMany(c => c.Order_table)
                .HasForeignKey(c => c.CustomerID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CustomerID");
            builder.HasOne(t => t.Table)
                .WithMany(t => t.Order_table)
                .HasForeignKey(t => t.TableID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TableID");
            builder.Property(t => t.From).IsRequired();
            builder.Property(t => t.To).IsRequired();
        }
    }
}