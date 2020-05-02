using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ManyToMany;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace LigthStreet.WebApi.Data
{
    public static class SeedData
    {
        public static void Initialize(LightStreetContext context,
            UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager)
        {
            var admin = new UserEntity()
            {
                UserName = "Admin",
                FirstName = "Admin",
                LastName = "Admin",
                ModifiedAt = DateTimeOffset.UtcNow
            };
            var adminResult = userManager.CreateAsync(admin, "AdminPass!&_777").Result;//why we need here result????

            #region Roles

            var adminRole = new RoleEntity()
            {
                Name = "Admin",
                CreatedById = admin.Id,
                CreatedAt = DateTimeOffset.UtcNow
            };
            var adminRoleResult = roleManager.CreateAsync(adminRole).Result;
            var addToRole = userManager.AddToRoleAsync(admin, adminRole.Name).Result;

            var grafanaUserRole = new RoleEntity()
            {
                Name = "GrafanaUser",
                CreatedById = admin.Id,
                CreatedAt = DateTimeOffset.UtcNow
            };
            var grafanaUserRoleResult = roleManager.CreateAsync(grafanaUserRole).Result;

            var userManagerRole = new RoleEntity()
            {
                Name = "UserManager",
                CreatedById = admin.Id,
                CreatedAt = DateTimeOffset.UtcNow
            };
            var userManagerRoleResult = roleManager.CreateAsync(userManagerRole).Result;

            var agentManagerRole = new RoleEntity()
            {
                Name = "AgentManager",
                CreatedById = admin.Id,
                CreatedAt = DateTimeOffset.UtcNow
            };
            var agentManagerRoleResult = roleManager.CreateAsync(agentManagerRole).Result;

            #endregion Roles

            #region Permission

            var allowAdmin = new PermissionEntity() { Description = "Permission which give access only for main admin", Id = 1, Name = "AllowAdmin" };
            allowAdmin.RolePermissions = new List<RolePermissionEntity>()
            {
                new RolePermissionEntity()
                {
                    RoleId = adminRole.Id
                }
            };
            context.Permissions.Add(allowAdmin);

            var allowGrafana = new PermissionEntity() { Description = "Permission which give access to grafana system", Id = 2, Name = "AllowGrafana" };
            allowGrafana.RolePermissions = new List<RolePermissionEntity>()
            {
                new RolePermissionEntity()
                {
                    RoleId = adminRole.Id
                },
                new RolePermissionEntity()
                {
                    RoleId = grafanaUserRole.Id
                }
            };
            context.Permissions.Add(allowGrafana);

            var allowUserManagement = new PermissionEntity() { Description = "Permission which give opportunity to manage user for example approve their or block also un register from system", Id = 3, Name = "AllowUserManagement" };
            allowUserManagement.RolePermissions = new List<RolePermissionEntity>()
            {
                new RolePermissionEntity()
                {
                    RoleId = adminRole.Id
                },
                new RolePermissionEntity()
                {
                    RoleId = userManagerRole.Id
                }
            };
            context.Permissions.Add(allowUserManagement);

            var allowAgentManagement = new PermissionEntity() { Description = "Permission which give opportunity to manage agent for example approve their or block also un register from system", Id = 4, Name = "AllowAgentManagement" };
            allowAgentManagement.RolePermissions = new List<RolePermissionEntity>()
            {
                new RolePermissionEntity()
                {
                    RoleId = adminRole.Id
                },
                new RolePermissionEntity()
                {
                    RoleId = agentManagerRole.Id
                }
            };
            context.Permissions.Add(allowAgentManagement);

            var allowAgentInstall = new PermissionEntity() { Description = "???? Developer has no idea which difference between this and next permission", Id = 5, Name = "AllowAgentInstall" };
            allowAgentInstall.RolePermissions = new List<RolePermissionEntity>()
            {
                new RolePermissionEntity()
                {
                    RoleId = adminRole.Id
                }
            };
            context.Permissions.Add(allowAgentInstall);

            var allowFirmwareInstall = new PermissionEntity() { Description = "????", Id = 6, Name = "AllowFirmwareInstall" };
            allowFirmwareInstall.RolePermissions = new List<RolePermissionEntity>()
            {
                new RolePermissionEntity()
                {
                    RoleId = adminRole.Id
                }
            };
            context.Permissions.Add(allowFirmwareInstall);
            using (var transaction = context.Database.BeginTransaction())
            {
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Permissions] ON");
                #endregion Permission
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Permissions] OFF");

                transaction.Commit();
            }
        }
    }
}
