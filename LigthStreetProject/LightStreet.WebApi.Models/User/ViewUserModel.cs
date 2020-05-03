using Domain.Enums;
using LightStreet.WebAPI.Models.Tag;
using System;
using System.Collections.Generic;

namespace LightStreet.WebAPI.Models.User
{
    public class ViewUserModel
    {
        public int Id { get; set; }
        public int? CreatedById { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public UserStatusType Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<ViewTagModel> Tags { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }
}