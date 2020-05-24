using Infrastructure.Attributes;
using Infrastructure.Models.Common;
using Infrastructure.Models.Enums;
using Infrastructure.Models.ManyToMany;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    [TableDescription("User")]
    public class UserEntity : IdentityUser<int>, IEntity
    {
        public int? ModifiedById { get; set; }
        public virtual UserEntity ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public UserStatusTypeEntity Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<UserTagEntity> UserTags { get; set; }
        public virtual List<UserEntity> CreatedUsers { get; set; }
        public virtual List<UserRoleEntity> UserRoles { get; set; }
        public virtual List<ReviewEntity> Reviews { get; set; }
        public virtual List<ReviewEntity> CreatedReviews { get; set; }
        #region overrides
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
            { EntityState.Added,"Added"},
            { EntityState.Modified,"Modified"},
            { EntityState.Deleted,"Un registered"},
        };
    }
}
