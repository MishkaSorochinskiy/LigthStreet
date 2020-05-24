using Infrastructure.Models;
using Infrastructure.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Infrastructure.Configuration
{
    public class PendingUserConfiguration : IEntityTypeConfiguration<PendingUserEntity>
    {
        public void Configure(EntityTypeBuilder<PendingUserEntity> builder)
        {
            builder.ToTable("PendingUsers");

            builder.HasKey(e => e.Id);

            builder.Property(x => x.Status)
                .HasConversion(x => x.ToString(),
                    x => (PendingTypeEntity)Enum.Parse(typeof(PendingTypeEntity), x));
        }
    }
}
