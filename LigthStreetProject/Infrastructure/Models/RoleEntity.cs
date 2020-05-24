using Infrastructure.Attributes;
using Infrastructure.Models.Common;
using Infrastructure.Models.ManyToMany;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    [TableDescription("Role")]
    public class RoleEntity : IdentityRole<int>, IEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public virtual UserEntity CreatedBy { get; set; }
        public virtual List<RolePermissionEntity> RolePermissions { get; set; }
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }


        public override string ConcurrencyStamp { get; set; }

        public override string NormalizedName { get; set; }

        public string GetAction(EntityState state)
        {
            return DictionaryActionName[state];
        }

        private static readonly Dictionary<EntityState, string> DictionaryActionName = new Dictionary<EntityState, string>()
        {
            { EntityState.Added,"Added"},
            { EntityState.Modified,"Modified"},
            { EntityState.Deleted,"Deleted"},
        };
    }
}
