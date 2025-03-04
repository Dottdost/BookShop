using BookShop.Data.Models;
namespace BookShop.Data.FluentConfigs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AttributeConfiguration : IEntityTypeConfiguration<BookAttribute>
{
    public void Configure(EntityTypeBuilder<BookAttribute> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired().HasMaxLength(255);
    }
}