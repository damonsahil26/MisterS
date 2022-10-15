using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using MisterS.Services.Identity.Models;
using System.Security.Claims;

namespace MisterS.Services.Identity.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProfileService(IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(subjectId);
            var userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

            var allClaims = userClaims.Claims.ToList();
            allClaims = allClaims.Where(c => context.RequestedClaimTypes.Contains(c.Type)).ToList();
            if (_userManager.SupportsUserRole)
            {
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var roleName in roles)
                {
                    allClaims.Add(new Claim(JwtClaimTypes.Role, roleName));
                    allClaims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
                    allClaims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
                    if (_roleManager.SupportsRoleClaims)
                    {
                        var role = await _roleManager.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            allClaims.AddRange(await _roleManager.GetClaimsAsync(role));
                        }
                    }
                }
            }
            context.IssuedClaims = allClaims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();

            var user = await _userManager.FindByIdAsync(subjectId);
            if (user != null)
            {
                context.IsActive = true;
            }
        }
    }
}
