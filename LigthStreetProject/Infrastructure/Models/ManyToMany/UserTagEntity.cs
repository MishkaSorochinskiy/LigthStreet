using Infrastructure.Attributes;
using Infrastructure.Models.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models.ManyToMany
{
    [TableDescription("User Tag")]
    public class UserTagEntity : IEntity
    {
        public int UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public int TagId { get; set; }
        public virtual TagEntity Tag { get; set; }

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
