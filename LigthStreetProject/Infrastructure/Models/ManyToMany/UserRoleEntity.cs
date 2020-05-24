using Infrastructure.Attributes;
using Infrastructure.Models.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models.ManyToMany
{
    [TableDescription("User Role")]
    public class UserRoleEntity : IdentityUserRole<int>, IEntity
    {
        public virtual UserEntity User { get; set; }
        public virtual RoleEntity Role { get; set; }
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
