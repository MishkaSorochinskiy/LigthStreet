using Infrastructure.Models.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.ManyToMany
{
    public class UserTagConfiguration : IEntityTypeConfiguration<UserTagEntity>
    {
        public void Configure(EntityTypeBuilder<UserTagEntity> builder)
        {
            builder.ToTable("UserTags");

            builder.HasKey(key => new { key.UserId, key.TagId });

            builder
                .HasOne(ao => ao.User)
                .WithMany(o => o.UserTags)
                .HasForeignKey(ao => ao.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ao => ao.Tag)
                .WithMany(o => o.UserTags)
                .HasForeignKey(ao => ao.TagId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
