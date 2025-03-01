using BookShop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.FluentConfigs;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => ur.UserRoleId);
        builder.HasOne(ur => ur.RoleNameRefNavigation)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleNameRef);
        builder.HasOne(ur => ur.UserNameRefNavigation)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserNameRef);
    }
}