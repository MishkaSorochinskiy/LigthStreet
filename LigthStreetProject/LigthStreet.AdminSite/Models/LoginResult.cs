using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LigthStreet.AdminSite.Models
{
    public class LoginResult
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public LoginResponse Response { get; set; }
        public IEnumerable<Claim> Permissions { get; set; }
    }
}
