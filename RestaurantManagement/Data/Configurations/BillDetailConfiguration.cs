using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Data.Configurations
{
    public class BillDetailConfiguration: IEntityTypeConfiguration<BillDetail>
    {
        public void Configure(EntityTypeBuilder<BillDetail> builder)
        {
            builder.ToTable("BILL_DETAIL");
            builder.HasKey(bd => new { bd.BillId, bd.FoodId });
            builder.Property(bd => bd.UnitPrice).IsRequired();
            builder.Property(bd => bd.Quantity).IsRequired();
            builder.Property(bd => bd.Price).IsRequired();

            builder.HasOne(bd => bd.Bill).WithMany(k => k.BillDetails).HasForeignKey(bd => bd.BillId);
            builder.HasOne(bd => bd.Food).WithMany(k => k.BillDetails).HasForeignKey(bd => bd.FoodId);
        }
    }
}
