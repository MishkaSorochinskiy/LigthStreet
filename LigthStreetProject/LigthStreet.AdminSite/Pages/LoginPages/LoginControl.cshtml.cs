using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LigthStreet.AdminSite.Models;
using LigthStreet.AdminSite.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IntegratedBiometrics.AdminSite
{
    public class LoginControlModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        public LoginModel Login { get; set; }

        public LoginControlModel(IAuthService authService)
        {
            _authService = authService;

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult>
            OnPostAsync()
        {
            string returnUrl = Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await HttpContext
                    .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }

            var result = await _authService.Login(Login);

            if (!result.Successful)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return Page();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Login.Email),
                new Claim("auth_token", result.Response.access_token)//TODO write extension for claim types for auth_token
            };
            claims.AddRange(result.Permissions);

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = this.Request.Host.Value
            };

            try
            {
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return LocalRedirect("/");
        }
    }
}