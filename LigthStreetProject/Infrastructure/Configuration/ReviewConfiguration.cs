using Infrastructure.Models;
using Infrastructure.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration
{
    public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewEntity> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(e => e.Id);

            builder.Property(p => p.Subject).IsRequired();

            builder.Property(p => p.Description).IsRequired();

            builder.Property(x => x.State)
               .HasConversion(x => x.ToString(),
                   x => (ReviewStateEntity)Enum.Parse(typeof(ReviewStateEntity), x));

            builder.HasOne(p => p.ApplyOn)
                .WithMany(u => u.Reviews)
                .HasForeignKey(t => t.ApplyOnId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.CreatedBy)
                .WithMany(u => u.CreatedReviews)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
