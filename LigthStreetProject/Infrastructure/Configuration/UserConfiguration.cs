using Infrastructure.Models;
using Infrastructure.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(e => e.Id);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(x => x.Status)
                .HasConversion(x => x.ToString(),
                    x => (UserStatusTypeEntity)Enum.Parse(typeof(UserStatusTypeEntity), x));

            builder.HasOne(task => task.ModifiedBy)
                .WithMany(category => category.CreatedUsers)
                .HasForeignKey(task => task.ModifiedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.HasMany(u => u.Reviews)
                .WithOne(ur => ur.ApplyOn)
                .HasForeignKey(ur => ur.ApplyOnId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.CreatedReviews)
               .WithOne(ur => ur.CreatedBy)
               .HasForeignKey(ur => ur.CreatedById)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
