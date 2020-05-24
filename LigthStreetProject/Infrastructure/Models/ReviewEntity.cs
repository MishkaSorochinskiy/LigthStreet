using Infrastructure.Attributes;
using Infrastructure.Models.Common;
using Infrastructure.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    [TableDescription("Reviews")]
    public class ReviewEntity: IEntity
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public ReviewStateEntity State { get; set; }
        public int? CreatedById { get; set; }
        public virtual UserEntity CreatedBy { get; set; }
        public int PointId { get; set; }
        public virtual PointEntity Point { get; set; }
        public int ApplyOnId { get; set; }
        public virtual UserEntity ApplyOn { get; set; }

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
