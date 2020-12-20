using HandiworkShop.DAL.Entities;
using Microsoft.AspNetCore.Identity;

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
        /// <param name="isVendor">Is vendor.</param>
        /// <returns>Identity result and user.</returns>
        System.Threading.Tasks.Task<(IdentityResult, ApplicationUser)> SignUpAsync(string email, string userName, string password, bool isVendor);

        /// <summary>
        /// Get user identifier by name.
        /// </summary>
        /// <param name="name">User name.</param>
        /// <returns>User identifier (GUID).</returns>
        System.Threading.Tasks.Task<string> GetUserIdByNameAsync(string name);

        /// <summary>
        /// Get user name by identifier.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Username.</returns>
        System.Threading.Tasks.Task<string> GetUserNameByIdAsync(string id);

        /// <summary>
        /// Change password.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Identity result.</returns>
        System.Threading.Tasks.Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    }
}