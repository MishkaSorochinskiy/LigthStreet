using Infrastructure.Attributes;
using Infrastructure.Models.Common;
using Infrastructure.Models.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    [TableDescription("Tag")]
    public class TagEntity : IEntity
    {
        public int Id { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string Name { get; set; }
 
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
        public virtual List<UserTagEntity> UserTags { get; set; }
    }
}
