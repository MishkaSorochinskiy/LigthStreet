using Domain.Models;
using Infrastructure.Configuration;
using Infrastructure.Configuration.ManyToMany;
using Infrastructure.Models;
using Infrastructure.Models.ManyToMany;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class LightStreetContext : IdentityDbContext<UserEntity,
        RoleEntity,
        int,
        IdentityUserClaim<int>,
        UserRoleEntity,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>>
    {

        public LightStreetContext(DbContextOptions<LightStreetContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Many to Many

            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserTagConfiguration());

            #endregion Many to Many

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PendingUserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new PointConfiguration());

        }

        /// <summary>
        /// Collection of all Users.

        /// <summary>
        /// Collection of all PendingUsers.
        /// </summary>
        public DbSet<PendingUserEntity> PendingUsers { get; set; }

        /// <summary>
        /// Collection of all Tags.
        /// </summary>
        public DbSet<TagEntity> Tags { get; set; }

        /// <summary>
        /// Collection of all AgentTags.
        /// </summary>
        public DbSet<PointEntity> AgentTags { get; set; }

        /// <summary>
        /// Collection of all UserTags.
        /// </summary>
        public DbSet<UserTagEntity> UserTags { get; set; }

        /// <summary>
        /// Collection of all Audit.
        /// </summary>
        public DbSet<RoleEntity> Roles { get; set; }

        /// <summary>
        /// Collection of all Audit.
        /// </summary>
        public DbSet<PermissionEntity> Permissions { get; set; }
    }
}
