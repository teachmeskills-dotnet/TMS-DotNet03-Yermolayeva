using HandiworkShop.BLL.Models;
using HandiworkShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Interfaces
{
    public interface IOrderManager
    {
        /// <summary>
        /// Create order.
        /// </summary>
        /// <param name="orderDto">Order data transfer object.</param>
        System.Threading.Tasks.Task CreateAsync(OrderDto orderDto);

        /// <summary>
        /// Get order by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Order data transfer objects.</returns>
        System.Threading.Tasks.Task<OrderDto> GetOrderAsync(int id);

        /// <summary>
        /// Get incoming orders by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>List of Order data transfer objects.</returns>
        System.Threading.Tasks.Task<IEnumerable<OrderDto>> GetIncomingOrdersAsync(string userId);

        /// <summary>
        /// Get outgoing orders by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>List of Order data transfer objects.</returns>
        System.Threading.Tasks.Task<IEnumerable<OrderDto>> GetOutgoingOrdersAsync(string userId);

        /// <summary>
        /// Delete order by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        System.Threading.Tasks.Task DeleteAsync(int id);

        /// <summary>
        /// Update order by identifier.
        /// </summary>
        /// <param name="orderDto">Task data transfer object.</param>
        System.Threading.Tasks.Task UpdateOrderAsync(OrderDto orderDto);

        /// <summary>
        /// Update order's status by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="state">State.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task UpdateOrderStatusAsync(int id, StateType state);
    }
}
