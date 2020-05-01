using Infrastructure.Models.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration.ManyToMany
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
    {
        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.ToTable("RolePermissions");

            builder.HasKey(key => new { key.RoleId, key.PermissionId });

            builder
                .HasOne(ao => ao.Role)
                .WithMany(o => o.RolePermissions)
                .HasForeignKey(ao => ao.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ao => ao.Permission)
                .WithMany(o => o.RolePermissions)
                .HasForeignKey(ao => ao.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
