using BookShop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.FluentConfigs;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.Id);

        // Указываем тип для ID и включаем автоинкремент
        builder.Property(g => g.Id)
            .HasColumnType("int")
            .ValueGeneratedOnAdd();

        builder.Property(g => g.GenreName)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("The name of the genre");

        builder.HasIndex(g => g.GenreName).IsUnique();

        builder.HasOne(g => g.ParentGenre)
            .WithMany(g => g.SubGenres)
            .HasForeignKey(g => g.ParentGenreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(g => g.ParentGenreId)
            .IsRequired(false);
    }
}