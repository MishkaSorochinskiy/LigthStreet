using Domain.Enums;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Infrastructure.Models;
using Infrastructure.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace LigthStreet.WebApi.Identity.Validators
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<UserEntity> _claimsFactory;
        private readonly UserManager<UserEntity> _userManager;
        public ProfileService(UserManager<UserEntity> userManager,
            IUserClaimsPrincipalFactory<UserEntity> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var grantType = context.Client.AllowedGrantTypes;
            if (grantType.Any(x => x == "password"))
            {
                var sub = context.Subject.GetSubjectId();
                var user = await _userManager.FindByIdAsync(sub);
                var principal = await _claimsFactory.CreateAsync(user);
                var claims = principal.Claims.ToList();
                claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
                var permissionList = user.UserRoles.SelectMany(x => x.Role.RolePermissions.Select(w => w.Permission.Name))
                    .ToList().Distinct();
                foreach (var permission in permissionList)
                {
                    claims.Add(new Claim("Permission", permission));
                }
                context.IssuedClaims = claims;
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var grantType = context.Client.AllowedGrantTypes;
            var sub = context.Subject.GetSubjectId();
            if (grantType.Any(x => x == "password"))
            {
                var user = await _userManager.FindByIdAsync(sub);
                context.IsActive = user != null;
                if ( user.IsDeleted || user.Status != UserStatusTypeEntity.Active)
                {
                    context.IsActive = false;
                }
            }
        }
    }
}
