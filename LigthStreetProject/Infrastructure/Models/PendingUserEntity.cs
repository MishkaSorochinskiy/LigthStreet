using Infrastructure.Attributes;
using Infrastructure.Models.Common;
using Infrastructure.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    [TableDescription("Pending user")]
    public class PendingUserEntity : IdentityUser<int>, IEntity
    {
        public DateTimeOffset ModifiedAt { get; set; }
        public PendingTypeEntity Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? LogViewSystemUserId { get; set; }

        #region overrides
        
        public override string UserName { get; set; }
        
        public override string NormalizedUserName { get; set; }
        
        public override string Email { get; set; }
        
        public override string NormalizedEmail { get; set; }
        
        public override string PasswordHash { get; set; }
        
        public override string SecurityStamp { get; set; }
        
        public override string ConcurrencyStamp { get; set; }
        
        public override string PhoneNumber { get; set; }
        
        public override bool PhoneNumberConfirmed { get; set; }
        
        public override bool TwoFactorEnabled { get; set; }
        
        public override DateTimeOffset? LockoutEnd { get; set; }
        
        public override bool LockoutEnabled { get; set; }
        
        public override int AccessFailedCount { get; set; }
        
        public override bool EmailConfirmed { get; set; }

        #endregion

        public string GetAction(EntityState state)
        {
            return DictionaryActionName[state];
        }

        private static readonly Dictionary<EntityState, string> DictionaryActionName = new Dictionary<EntityState, string>()
        {
            { EntityState.Added,null},
            { EntityState.Modified,null},
            { EntityState.Deleted,null},
        };
    }
}
