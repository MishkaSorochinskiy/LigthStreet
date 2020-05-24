namespace LightStreet.WebApi.Models.User
{
    public class ChangePasswordRequestModel
    {
        public ChangePasswordRequestModel(string oldPassword,
            string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public string OldPassword { get; }
        public string NewPassword { get; }
    }
}