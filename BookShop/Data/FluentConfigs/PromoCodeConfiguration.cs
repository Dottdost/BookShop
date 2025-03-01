using BookShop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.FluentConfigs;

public class PromoCodeConfiguration : IEntityTypeConfiguration<PromoCode>
{
    public void Configure(EntityTypeBuilder<PromoCode> builder)
    {
        builder.HasKey(pc => pc.Id);
        builder.Property(pc => pc.Code).IsRequired().HasMaxLength(50);
        builder.Property(pc => pc.Discount).HasColumnType("decimal(5,2)");
        builder.Property(pc => pc.ExpiryDate).IsRequired();
    }
}