using HandiworkShop.BLL.Models;
using System;
using System.Collections.Generic;

namespace HandiworkShop.BLL.Interfaces
{
    /// <summary>
    /// Task manager.
    /// </summary>
    public interface ITaskManager
    {
        /// <summary>
        /// Create task.
        /// </summary>
        /// <param name="taskDto">Task data transfer object.</param>
        /// <param name="userId">User identifier.</param>
        System.Threading.Tasks.Task CreateAsync(TaskDto taskDto, string userId);

        /// <summary>
        /// Get task by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userId">User identifier.</param>
        /// <returns>Task data transfer objects.</returns>
        System.Threading.Tasks.Task<TaskDto> GetTaskAsync(int id, string userId);

        /// <summary>
        /// Get order's tasks by order identifier.
        /// </summary>
        /// <param name="id">Order identifier.</param>
        /// <param name="userId">User identifier.</param>
        /// <returns>Collection of task data transfer objects.</returns>
        System.Threading.Tasks.Task<IEnumerable<TaskDto>> GetOrderTasksAsync(int orderId, string userId);

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
        /// <param name="userId">User identifier.</param>
        System.Threading.Tasks.Task DeleteAsync(int id, string userId);

        /// <summary>
        /// Update task by identifier.
        /// </summary>
        /// <param name="taskDto">Task data transfer object.</param>
        /// <param name="userId">User identifier.</param>
        System.Threading.Tasks.Task UpdateTaskAsync(TaskDto taskDto, string userId);

        /// <summary>
        /// Update task's state by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userId">User identifier.</param>
        System.Threading.Tasks.Task UpdateTaskStateAsync(int id, string userId);
    }
}