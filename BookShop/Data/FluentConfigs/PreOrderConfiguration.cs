using BookShop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.FluentConfigs;

public class PreOrderConfiguration : IEntityTypeConfiguration<PreOrder>
{
    public void Configure(EntityTypeBuilder<PreOrder> builder)
    {
        builder.HasKey(po => po.Id);
        
        builder.Property(po => po.UserName)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(po => po.Title)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(po => po.Author)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasIndex(po => new { po.UserName, po.Title, po.Author }).IsUnique();
    }
}