using HandiworkShop.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.BLL.Interfaces
{
    public interface IProfileManager
    {
        /// <summary>
        /// Create profile.
        /// </summary>
        /// <param name="profileDto">Profile data transfer object.</param>
        System.Threading.Tasks.Task CreateAsync(ProfileDto profileDto);

        /// <summary>
        /// Get profile by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userId">User identifier.</param>
        /// <returns>Profile data transfer objects.</returns>
        System.Threading.Tasks.Task<ProfileDto> GetProfileAsync(int id, string userId);

        /// <summary>
        /// Delete profile by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userId">User identifier.</param>
        System.Threading.Tasks.Task DeleteAsync(int id, string userId);

        /// <summary>
        /// Update profile by identifier.
        /// </summary>
        /// <param name="profileDto">Profile data transfer object.</param>
        System.Threading.Tasks.Task UpdateProfileAsync(ProfileDto profileDto);

        /// <summary>
        /// Get all vendor profiles.
        /// </summary>
        /// <returns>A collection of vendor profiles.</returns>
        System.Threading.Tasks.Task<IEnumerable<ProfileDto>> GetAllVendorProfiles();
    }
}
