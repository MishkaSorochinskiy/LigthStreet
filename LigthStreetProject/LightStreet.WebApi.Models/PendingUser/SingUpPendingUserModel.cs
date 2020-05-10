namespace LightStreet.WebApi.Models.PendingUser
{
    public class SingUpPendingUserModel
    {
        public SingUpPendingUserModel(string password,
            string login, string firstName,
            string lastName, string email)
        {
            Password = password;
            Login = login;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string Password { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}