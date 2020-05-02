using Domain.Models.ManyToMany;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public virtual User CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public List<UserRole> UserRole { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
    }
}
