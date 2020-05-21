using LigthStreet.AdminSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigthStreet.AdminSite.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ActionResult> Register(RegisterModel registerModel);
        Task<LoginResult> Login(LoginModel loginModel);
    }
}
