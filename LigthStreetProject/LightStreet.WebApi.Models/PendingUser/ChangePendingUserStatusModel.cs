using Domain.Enums;
using System.Collections.Generic;

namespace LightStreet.WebApi.Models.PendingUser
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