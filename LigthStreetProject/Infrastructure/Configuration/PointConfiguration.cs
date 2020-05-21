using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration
{
    public class PointConfiguration : IEntityTypeConfiguration<PointEntity>
    {
        public void Configure(EntityTypeBuilder<PointEntity> builder)
        {
            builder.ToTable("Points");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Latitude).IsRequired();

            builder.Property(e => e.Longtitude).IsRequired();

            builder.HasMany(u => u.Reviews)
               .WithOne(ur => ur.Point)
               .HasForeignKey(ur => ur.PointId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
