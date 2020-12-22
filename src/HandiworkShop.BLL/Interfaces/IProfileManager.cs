using HandiworkShop.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Interfaces
{
    /// <summary>
    /// Profile manager.
    /// </summary>
    public interface IProfileManager
    {
        /// <summary>
        /// Create profile.
        /// </summary>
        /// <param name="profileDto">Profile data transfer object.</param>
        Task CreateAsync(ProfileDto profileDto);

        /// <summary>
        /// Get profile by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Profile data transfer object.</returns>
        Task<ProfileDto> GetProfileAsync(string userId);

        /// <summary>
        /// Delete profile by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userId">User identifier.</param>
        Task DeleteAsync(int id, string userId);

        /// <summary>
        /// Update profile by identifier.
        /// </summary>
        /// <param name="profileDto">Profile data transfer object.</param>
        /// <param name="userId">User identifier.</param>
        Task UpdateProfileAsync(ProfileDto profileDto, string userId);

        /// <summary>
        /// Switch profile status by identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        Task SwitchProfileStatusAsync(string userId);

        /// <summary>
        /// Get profiles by tags.
        /// </summary>
        /// <param name="tagIds">List of tag identifiers.</param>
        /// <returns>List of profile data transfer objects.</returns>
        Task<IEnumerable<ProfileDto>> GetProfilesByTagsAsync(IList<int> tagIds);
    }
}