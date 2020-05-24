using System;
using System.Collections.Generic;
using System.Text;

namespace LightStreet.Models.PendingUser
{
    public class ChangePendingUserStatusModel
    {
        public ChangePendingUserStatusModel(List<string> tags,
            UserStatusType status,
            int userId,
            int roleId)
        {
            Tags = tags;
            Status = status;
            UserId = userId;
            RoleId = roleId;
        }

        public List<string> Tags { get; }
        public UserStatusType Status { get; }
        public int UserId { get; }
        public int RoleId { get; }
    }
}
