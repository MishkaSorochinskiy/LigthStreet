using Domain.Enums;

namespace LightStreet.WebApi.Models.User
{
    public class ChangeUserStatusModel
    {
        public ChangeUserStatusModel(UserStatusType status,
            int userId)
        {
            Status = status;
            UserId = userId;
        }

        public UserStatusType Status { get; }
        public int UserId { get; }
    }
}