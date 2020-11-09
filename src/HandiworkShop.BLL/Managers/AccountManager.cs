using Microsoft.AspNetCore.Identity;
using HandiworkShop.BLL.Interfaces;
using HandiworkShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HandiworkShop.BLL.Models;

namespace HandiworkShop.BLL.Managers
{
    ///<inheritdoc cref="IAccountManager"/>
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfileManager _profileManager;

        public AccountManager(UserManager<ApplicationUser> userManager, IProfileManager profileManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _profileManager = profileManager ?? throw new ArgumentNullException(nameof(profileManager));
        }

        public async System.Threading.Tasks.Task<(IdentityResult, ApplicationUser)> SignUpAsync(string email, string userName, string password, bool isVendor)
        {
            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = userName,
                Email = email,
            };

            var result = await _userManager.CreateAsync(applicationUser, password);
            if (result.Succeeded)
            {
                await _profileManager.CreateAsync(new ProfileDto
                {
                    IsVendor = isVendor,
                    UserId = (await _userManager.FindByNameAsync(userName)).Id,
                    Name = userName,
                    Info = null
                }); ;
            }
            return (result, applicationUser);
        }
    }
}
