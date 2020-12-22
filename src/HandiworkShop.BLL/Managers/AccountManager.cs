using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.Common.Constants;
using HandiworkShop.Common.Resourses;
using HandiworkShop.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Managers
{
    ///<inheritdoc cref="IAccountManager"/>
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfileManager _profileManager;

        public AccountManager(
            UserManager<ApplicationUser> userManager,
            IProfileManager profileManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _profileManager = profileManager ?? throw new ArgumentNullException(nameof(profileManager));
        }

        public async System.Threading.Tasks.Task<(IdentityResult, ApplicationUser)> SignUpAsync(
            string email,
            string userName,
            string password,
            bool isVendor)
        {
            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = userName,
                Email = email,
            };

            var result = await _userManager.CreateAsync(applicationUser, password);
            if (result.Succeeded)
            {
                if (isVendor)
                {
                    await _userManager.AddToRoleAsync(applicationUser, RolesConstants.VendorRole);
                }
                await _profileManager.CreateAsync(new ProfileDto
                {
                    IsVendor = isVendor,
                    UserId = applicationUser.Id,
                    Name = userName,
                    Info = null
                });
            }
            return (result, applicationUser);
        }

        public async Task<string> GetUserIdByNameAsync(string name)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == name);
            if (user is null)
            {
                throw new KeyNotFoundException(ErrorResource.UserNotFound);
            }
            return user.Id;
        }

        public async Task<string> GetUserNameByIdAsync(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (user is null)
            {
                throw new KeyNotFoundException(ErrorResource.UserNotFound);
            }
            return user.UserName;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException(ErrorResource.UserNotFound);
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result;
        }
    }
}