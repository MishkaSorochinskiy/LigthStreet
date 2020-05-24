using Infrastructure.Attributes;
using Infrastructure.Models.Common;
using Infrastructure.Models.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    [TableDescription("Permission")]
    public class PermissionEntity : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<RolePermissionEntity> RolePermissions { get; set; }

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
