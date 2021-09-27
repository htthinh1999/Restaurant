using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.Data.Entities;

namespace RestaurantManagement.Data.Configurations
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.ToTable("TABLE");
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Name).IsRequired().IsUnicode().HasMaxLength(30);
            builder.Property(table => table.PeopleCount).IsRequired();
        }
    }
}
