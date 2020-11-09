using HandiworkShop.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.BLL.Interfaces
{
    public interface ITagManager
    {
        /// <summary>
        /// Create tag.
        /// </summary>
        /// <param name="tagDto">Tag data transfer object.</param>
        System.Threading.Tasks.Task CreateAsync(TagDto tagDto);

        /// <summary>
        /// Get tag by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Tag data transfer objects.</returns>
        System.Threading.Tasks.Task<TagDto> GetTagAsync(int id);

        /// <summary>
        /// Get user's tags by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>List of Tag data transfer objects.</returns>
        System.Threading.Tasks.Task<IEnumerable<TagDto>> GetTagsAsync(string userId);

        /// <summary>
        /// Delete tag by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        System.Threading.Tasks.Task DeleteAsync(int id);

        /// <summary>
        /// Add user a tag.
        /// </summary>
        /// <param name="id">Identifier..</param>
        /// <param name="userId">User identifier.</param>
        System.Threading.Tasks.Task AddUserTagAsync(int id, string userId);

        /// <summary>
        /// Delete user's tag.
        /// </summary>
        /// <param name="id">Identifier..</param>
        /// <param name="userId">User identifier.</param>
        System.Threading.Tasks.Task DeleteUserTagAsync(int id, string userId);

        /// <summary>
        /// Update tag by identifier.
        /// </summary>
        /// <param name="tagDto">Tag data transfer object.</param>
        System.Threading.Tasks.Task UpdateTagAsync(TagDto tagDto);
    }
}
