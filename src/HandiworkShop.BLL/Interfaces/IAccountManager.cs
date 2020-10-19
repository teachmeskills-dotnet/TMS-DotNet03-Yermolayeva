using Microsoft.AspNetCore.Identity;
using HandiworkShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Interfaces
{
    /// <summary>
    /// Account manager.
    /// </summary>
    public interface IAccountManager
    {
        /// <summary>
        /// Sign up.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="userName">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns>Identity result and user.</returns>
        System.Threading.Tasks.Task<(IdentityResult, ApplicationUser)> SignUpAsync(string email, string userName, string password);
    }
}
