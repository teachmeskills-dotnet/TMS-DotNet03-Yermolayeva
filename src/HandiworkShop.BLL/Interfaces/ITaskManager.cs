using HandiworkShop.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.BLL.Interfaces
{
    public interface ITaskManager
    {
        /// <summary>
        /// Create profile.
        /// </summary>
        /// <param name="taskDto">Task data transfer object.</param>
        System.Threading.Tasks.Task CreateAsync(TaskDto taskDto);

        /// <summary>
        /// Get task by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Task data transfer objects.</returns>
        System.Threading.Tasks.Task<TaskDto> GetTaskAsync(int id);

        /// <summary>
        /// Get order's tasks by order identifier.
        /// </summary>
        /// <param name="id">Order identifier.</param>
        /// <returns>Collection of task data transfer objects.</returns>
        System.Threading.Tasks.Task<IEnumerable<TaskDto>> GetOrderTasksAsync(int orderId);

        /// <summary>
        /// Get user's tasks by user identifier and date.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="date">Date.</param>
        /// <returns>Collection of task data transfer objects.</returns>
        System.Threading.Tasks.Task<IEnumerable<TaskDto>> GetUserTasksByDateAsync(string userId, DateTime date);

        /// <summary>
        /// Delete task by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        System.Threading.Tasks.Task DeleteAsync(int id);

        /// <summary>
        /// Update task by identifier.
        /// </summary>
        /// <param name="taskDto">Task data transfer object.</param>
        System.Threading.Tasks.Task UpdateTaskAsync(TaskDto taskDto);
    }
}
