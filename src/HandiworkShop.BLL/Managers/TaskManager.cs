﻿using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.Common.Enums;
using HandiworkShop.Common.Resourses;
using HandiworkShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HandiworkShop.BLL.Managers
{
    ///<inheritdoc cref="ITaskManager"/>
    public class TaskManager : ITaskManager
    {
        private readonly IRepository<DAL.Entities.Task> _repositoryTask;
        private readonly IRepository<Order> _repositoryOrder;

        public TaskManager(
            IRepository<DAL.Entities.Task> repositoryTask,
            IRepository<Order> repositoryOrder)
        {
            _repositoryTask = repositoryTask ?? throw new ArgumentNullException(nameof(repositoryTask));
            _repositoryOrder = repositoryOrder ?? throw new ArgumentNullException(nameof(repositoryOrder));
        }

        public async System.Threading.Tasks.Task CreateAsync(TaskDto taskDto, string userId)
        {
            taskDto = taskDto ?? throw new ArgumentNullException(nameof(taskDto));

            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == taskDto.OrderId
                && order.VendorId == userId && order.State == StateType.InProcess);

            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            var task = new Task
            {
                OrderId = taskDto.OrderId,
                Description = taskDto.Description,
                Start = taskDto.Start,
                End = taskDto.End,
                Title = taskDto.Title,
                State = StateType.InProcess
            };

            await _repositoryTask.CreateAsync(task);
            await _repositoryTask.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id, string userId)
        {
            var task = await _repositoryTask.GetEntityAsync(task => task.Id == id);
            if (task is null)
            {
                throw new KeyNotFoundException(ErrorResource.TaskNotFound);
            }

            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == task.OrderId && order.VendorId == userId);
            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            _repositoryTask.Delete(task);
            await _repositoryTask.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskDto>> GetOrderTasksAsync(int orderId, string userId)
        {
            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == orderId && order.VendorId == userId);
            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            var taskDtos = new List<TaskDto>();
            var tasks = await _repositoryTask
                .GetAll()
                .AsNoTracking()
                .Where(task => task.OrderId == orderId)
                .ToListAsync();

            if (tasks.Any())
            {
                foreach (var task in tasks)
                {
                    taskDtos.Add(new TaskDto
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Start = task.Start,
                        End = task.End,
                        State = task.State,
                        OrderId = task.OrderId
                    });
                }
            }

            return taskDtos;
        }

        public async System.Threading.Tasks.Task<TaskDto> GetTaskAsync(int id, string userId)
        {
            var task = await _repositoryTask.GetEntityAsync(task => task.Id == id);
            if (task is null)
            {
                throw new KeyNotFoundException(ErrorResource.TaskNotFound);
            }

            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == task.OrderId && order.VendorId == userId);
            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            var taskDto = new TaskDto
            {
                Id = task.Id,
                Description = task.Description,
                End = task.End,
                Start = task.Start,
                OrderId = task.OrderId,
                State = task.State,
                Title = task.Title
            };
            return taskDto;
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskDto>> GetUserTasksByDateAsync(string userId, DateTime date)
        {
            var orderIds = await _repositoryOrder
                .GetAll()
                .AsNoTracking()
                .Where(order => order.VendorId == userId && order.State == StateType.InProcess)
                .Select(order => order.Id)
                .ToListAsync();

            var taskDtos = new List<TaskDto>();

            var tasks = await _repositoryTask
                .GetAll()
                .AsNoTracking()
                .Where(task => orderIds.Contains(task.OrderId) && date >= task.Start && date <= task.End)
                .ToListAsync();

            if (tasks.Any())
            {
                foreach (var task in tasks)
                {
                    taskDtos.Add(new TaskDto
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Start = task.Start,
                        End = task.End,
                        State = task.State,
                        OrderId = task.OrderId
                    });
                }
            }

            return taskDtos;
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(TaskDto taskDto, string userId)
        {
            taskDto = taskDto ?? throw new ArgumentNullException(nameof(taskDto));
            var task = await _repositoryTask.GetEntityAsync(task => task.Id == taskDto.Id);
            if (task is null)
            {
                throw new KeyNotFoundException(ErrorResource.TaskNotFound);
            }

            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == task.OrderId && order.VendorId == userId);
            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            static bool ValidateToUpdate(Task task, TaskDto taskDto)
            {
                bool updated = false;

                if (task.Title != taskDto.Title)
                {
                    task.Title = taskDto.Title;
                    updated = true;
                }

                if (task.Description != taskDto.Description)
                {
                    task.Description = taskDto.Description;
                    updated = true;
                }

                if (task.End != taskDto.End)
                {
                    task.End = taskDto.End;
                    updated = true;
                }

                if (task.Start != taskDto.Start)
                {
                    task.Start = taskDto.Start;
                    updated = true;
                }

                return updated;
            }

            var result = ValidateToUpdate(task, taskDto);
            if (result)
            {
                await _repositoryTask.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task UpdateTaskStateAsync(int id, string userId)
        {
            var task = await _repositoryTask.GetEntityAsync(task => task.Id == id);
            if (task is null)
            {
                throw new KeyNotFoundException(ErrorResource.TaskNotFound);
            }

            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == task.OrderId && order.VendorId == userId);
            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            if (task.State == StateType.InProcess)
            {
                task.State = StateType.Completed;
                if (task.End == null)
                {
                    task.End = DateTime.Now.Date;
                }
            }
            else
            {
                task.State = StateType.InProcess;
            }

            await _repositoryTask.SaveChangesAsync();
        }
    }
}