using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookShop.Data.Models;

public class BookAttributeValueConfig : IEntityTypeConfiguration<BookAttributeValue>
{
    public void Configure(EntityTypeBuilder<BookAttributeValue> builder)
    {
        builder.HasKey(bav => bav.Id);

        builder.Property(bav => bav.Value)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(bav => bav.Book)
            .WithMany(b => b.BookAttributeValues)
            .HasForeignKey(bav => bav.BookId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(bav => bav.Attribute)
            .WithMany() 
            .HasForeignKey(bav => bav.AttributeId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}