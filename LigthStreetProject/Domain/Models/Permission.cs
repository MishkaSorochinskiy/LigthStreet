using Domain.Models.ManyToMany;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<RolePermission> RolePermissions { get; set; }
    }
}