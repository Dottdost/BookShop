using BookShop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.FluentConfigs;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(w => w.Id);

        builder.HasOne(w => w.Book)
            .WithOne(b => b.Warehouse)
            .HasForeignKey<Warehouse>(w => w.BookId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.Property(w => w.Quantity)
            .IsRequired(); 
        
        builder.Property(w => w.Quantity)
            .HasDefaultValue(0); 

        builder.Property(w => w.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()") 
            .ValueGeneratedOnAddOrUpdate(); 
    }
}