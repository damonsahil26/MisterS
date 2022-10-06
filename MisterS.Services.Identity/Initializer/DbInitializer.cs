using IdentityModel;
using Microsoft.AspNetCore.Identity;
using MisterS.Services.Identity.DbContexts;
using MisterS.Services.Identity.Models;
using System.Security.Claims;

namespace MisterS.Services.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_roleManager.FindByNameAsync(StaticData.Constants.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(StaticData.Constants.Admin))
                    .GetAwaiter()
                    .GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticData.Constants.Customer))
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                return;
            }

            var adminUser = new ApplicationUser
            {
                Email = "adminsahil@gmail.com",
                UserName = "adminsahil@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                FirstName = "Sahil",
                LastName = "Malhotra"
            };

            _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, StaticData.Constants.Admin).GetAwaiter().GetResult();
            var temp1 = _userManager.AddClaimsAsync(adminUser, new Claim[]
             {
                new Claim(JwtClaimTypes.Name,adminUser.FirstName + " " + adminUser.LastName),
                new Claim(JwtClaimTypes.Email,adminUser.Email),
                new Claim(JwtClaimTypes.PhoneNumber,adminUser.PhoneNumber),
                new Claim(JwtClaimTypes.FamilyName,adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName,adminUser.FirstName),
                new Claim(JwtClaimTypes.Role,StaticData.Constants.Admin)
             }).GetAwaiter().GetResult();

            var customerUser = new ApplicationUser
            {
                Email = "customersahil@gmail.com",
                UserName = "customersahil@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "987654321",
                FirstName = "Sahil",
                LastName = "Malhotra"
            };

            _userManager.CreateAsync(customerUser, "Customer123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, StaticData.Constants.Customer).GetAwaiter().GetResult();
            var temp2 = _userManager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,customerUser.FirstName + " " + customerUser.LastName),
                new Claim(JwtClaimTypes.Email,customerUser.Email),
                new Claim(JwtClaimTypes.PhoneNumber,customerUser.PhoneNumber),
                new Claim(JwtClaimTypes.FamilyName,customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName,customerUser.FirstName),
                new Claim(JwtClaimTypes.Role,StaticData.Constants.Customer)
            }).Result;
        }
    }
}
