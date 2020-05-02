using Domain.Enums;
using Domain.Models.ManyToMany;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public int? ModifiedById { get; set; }
        public virtual User ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public UserStatusType Status { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int? LogViewSystemUserId { get; set; }
        public bool IsDeleted { get; set; }
        public List<UserTag> UserTags { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
