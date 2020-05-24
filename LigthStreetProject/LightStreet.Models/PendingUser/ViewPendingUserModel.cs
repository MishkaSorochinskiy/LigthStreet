
using System;

namespace IntegratedBiometrics.WebAPI.Models.PendingUser
{
    public class ViewPendingUserModel
    {
        public ViewPendingUserModel(int id,
            DateTimeOffset modifiedAt,
            PendingType status,
            string firstName,
            string lastName,
            string email,
            string userName)
        {
            Id = id;
            ModifiedAt = modifiedAt;
            Status = status;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = userName;
        }

        public int Id { get; }
        public DateTimeOffset ModifiedAt { get; }
        public PendingType Status { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string UserName { get; }
    }
}