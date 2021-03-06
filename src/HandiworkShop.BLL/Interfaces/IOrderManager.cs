﻿using HandiworkShop.BLL.Models;
using HandiworkShop.Common.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Interfaces
{
    /// <summary>
    /// Order manager.
    /// </summary>
    public interface IOrderManager
    {
        /// <summary>
        /// Create order.
        /// </summary>
        /// <param name="orderDto">Order data transfer object.</param>
        Task CreateAsync(OrderDto orderDto);

        /// <summary>
        /// Get order by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Order data transfer object.</returns>
        Task<OrderDto> GetOrderAsync(int id);

        /// <summary>
        /// Get incoming orders by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>List of order data transfer objects.</returns>
        Task<IEnumerable<OrderDto>> GetIncomingOrdersAsync(string userId);

        /// <summary>
        /// Get outgoing orders by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>List of order data transfer objects.</returns>
        Task<IEnumerable<OrderDto>> GetOutgoingOrdersAsync(string userId);

        /// <summary>
        /// Delete order by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userId">User identifier.</param>
        Task DeleteAsync(int id, string userId);

        /// <summary>
        /// Update order by identifier.
        /// </summary>
        /// <param name="orderDto">Order data transfer object.</param>
        /// <param name="userId">User identifier.</param>
        Task UpdateOrderAsync(OrderDto orderDto, string userId);

        /// <summary>
        /// Update order's state by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="state">State.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="vendorId">Vendor's user identifier.</param>
        Task UpdateOrderStateAsync(int id, StateType state, string userId, string vendorId = null);

        /// <summary>
        /// Create comment for order by identifier.
        /// </summary>
        /// <param name="commentDto">Comment data transfer object.</param>
        /// <param name="userId">User identifier.</param>
        Task CreateOrderCommentAsync(CommentDto commentDto, string userId);

        /// <summary>
        /// Get order comment by order identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userId">User identifier.</param>
        /// <returns>Comment data transfer object.</returns>
        Task<CommentDto> GetOrderCommentAsync(int id, string userId);

        /// <summary>
        /// Update comment for order by identifier.
        /// </summary>
        /// <param name="commentDto">Comment data transfer object.</param>
        /// <param name="userId">User identifier.</param>
        Task UpdateOrderCommentAsync(CommentDto commentDto, string userId);

        /// <summary>
        /// Delete order comment by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userId">User identifier.</param>
        Task DeleteOrderCommentAsync(int id, string userId);

        /// <summary>
        /// Get user's comments by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Collection of comment data transfer objects.</returns>
        Task<IEnumerable<CommentDto>> GetUserCommentsAsync(string userId);

        /// <summary>
        /// Get orders by tags.
        /// </summary>
        /// <param name="tagIds">List of tag identifiers.</param>
        /// <returns>Collection of order data transfer objects.</returns>
        Task<IEnumerable<OrderDto>> GetOrdersByTagsAsync(IList<int> tagIds);
    }
}