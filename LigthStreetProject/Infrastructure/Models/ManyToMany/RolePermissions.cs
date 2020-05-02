using Infrastructure.Attributes;
using Infrastructure.Models.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Models.ManyToMany
{
    [TableDescription("Role Permission")]
    public class RolePermissionEntity : IEntity
    {
        public int RoleId { get; set; }
        public virtual RoleEntity Role { get; set; }
        public int PermissionId { get; set; }
        public virtual PermissionEntity Permission { get; set; }
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
