namespace LightStreet.WebAPI.Models.User
{
    public class ChangeUserRoleModel
    {
        public ChangeUserRoleModel(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public int UserId { get; }
        public int RoleId { get; }
    }
}