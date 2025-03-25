using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookShop.Data.Models;

namespace BookShop.Data.FluentConfigs;

public class BookAttributeConfiguration : IEntityTypeConfiguration<BookAttribute>
{
    public void Configure(EntityTypeBuilder<BookAttribute> builder)
    {
        builder.HasKey(ba => ba.Id);

        builder.Property(ba => ba.Name)
            .IsRequired()
            .HasMaxLength(255);

      
        builder.HasIndex(ba => ba.Name).IsUnique();

        builder.HasMany(ba => ba.BookAttributeValues)
            .WithOne(bav => bav.Attribute)
            .HasForeignKey(bav => bav.AttributeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}