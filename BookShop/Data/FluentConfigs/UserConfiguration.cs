using BookShop.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.FluentConfigs;

public class UserConfiguration
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserName);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
    }
}
