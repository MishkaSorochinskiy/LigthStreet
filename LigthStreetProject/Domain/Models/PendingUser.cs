using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class PendingUser
    {
            public int Id { get; set; }
            public DateTimeOffset ModifiedAt { get; set; }
            public PendingType Status { get; set; }
            public string PasswordHash { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
            public int? LogViewSystemUserId { get; set; }
    }
}
