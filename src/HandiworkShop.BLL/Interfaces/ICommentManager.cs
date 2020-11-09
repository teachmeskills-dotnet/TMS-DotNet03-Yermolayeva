using HandiworkShop.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.BLL.Interfaces
{
    public interface ICommentManager
    {
        /// <summary>
        /// Create comment.
        /// </summary>
        /// <param name="commentDto">Comment data transfer object.</param>
        System.Threading.Tasks.Task CreateAsync(CommentDto commentDto);

        /// <summary>
        /// Get comment by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userId">User identifier.</param>
        /// <returns>Comment data transfer objects.</returns>
        System.Threading.Tasks.Task<CommentDto> GetCommentAsync(int id);

        /// <summary>
        /// Get comments by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>List of Comment data transfer objects.</returns>
        System.Threading.Tasks.Task<IEnumerable<CommentDto>> GetCommentsAsync(string userId);

        /// <summary>
        /// Delete comment by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userId">User identifier.</param>
        System.Threading.Tasks.Task DeleteAsync(int id);

        /// <summary>
        /// Update comment by identifier.
        /// </summary>
        /// <param name="commentDto">Comment data transfer object.</param>
        System.Threading.Tasks.Task UpdateCommentAsync(CommentDto commentDto);
    }
}
