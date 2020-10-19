using Microsoft.AspNetCore.Identity;
using HandiworkShop.BLL.Interfaces;
using HandiworkShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Managers
{
    ///<inheritdoc cref="IAccountManager"/>
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async System.Threading.Tasks.Task<(IdentityResult, ApplicationUser)> SignUpAsync(string email, string userName, string password)
        {
            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = userName,
                Email = email,
            };

            var result = await _userManager.CreateAsync(applicationUser, password);
            return (result, applicationUser);
        }
    }
}
