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
        System.Threading.Tasks.Task<IEnumerable<TagDto>> GetUserTagsAsync(string userId);

        /// <summary>
        /// Get all tags.
        /// </summary>
        /// <returns>List of Tag data transfer objects.</returns>
        System.Threading.Tasks.Task<IEnumerable<TagDto>> GetAllTagsAsync();

        /// <summary>
        /// Get order's tags by user identifier.
        /// </summary>
        /// <param name="orderId">Order identifier.</param>
        /// <returns>List of Tag data transfer objects.</returns>
        System.Threading.Tasks.Task<IEnumerable<TagDto>> GetOrderTagsAsync(int orderId);

        /// <summary>
        /// Delete tag by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        System.Threading.Tasks.Task DeleteAsync(int id);

        /// <summary>
        /// Update user's tags.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="tagIds">List of tag ids.</param>
        System.Threading.Tasks.Task UpdateUserTagsAsync(string userId, IList<int> tagIds);

        /// <summary>
        /// Update order's tags.
        /// </summary>
        /// <param name="orderId">Order identifier.</param>
        /// <param name="tagIds">List of tag ids.</param>
        /// <param name="userId">User identifier.</param>
        System.Threading.Tasks.Task UpdateOrderTagsAsync(int orderId, IList<int> tagIds, string userId);

        /// <summary>
        /// Update tag by identifier.
        /// </summary>
        /// <param name="tagDto">Tag data transfer object.</param>
        System.Threading.Tasks.Task UpdateTagAsync(TagDto tagDto);
    }
}
