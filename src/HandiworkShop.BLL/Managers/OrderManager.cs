﻿using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.Common.Enums;
using HandiworkShop.Common.Resourses;
using HandiworkShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Managers
{
    ///<inheritdoc cref="IOrderManager"/>
    public class OrderManager : IOrderManager
    {
        private readonly IRepository<Order> _repositoryOrder;
        private readonly IRepository<DAL.Entities.Task> _repositoryTask;
        private readonly IRepository<OrderTag> _repositoryOrderTag;

        public OrderManager(
            IRepository<Order> repositoryOrder,
            IRepository<DAL.Entities.Task> repositoryTask,
            IRepository<OrderTag> repositoryOrderTag)
        {
            _repositoryOrder = repositoryOrder ?? throw new ArgumentNullException(nameof(repositoryOrder));
            _repositoryTask = repositoryTask ?? throw new ArgumentNullException(nameof(repositoryTask));
            _repositoryOrderTag = repositoryOrderTag ?? throw new ArgumentNullException(nameof(repositoryOrderTag));
        }

        public async System.Threading.Tasks.Task CreateAsync(OrderDto orderDto)
        {
            orderDto = orderDto ?? throw new ArgumentNullException(nameof(orderDto));

            var order = new Order
            {
                Title = orderDto.Title,
                Description = orderDto.Description,
                Start = DateTime.Now,
                End = orderDto.End,
                ClientId = orderDto.ClientId,
                VendorId = orderDto.VendorId,
                Price = orderDto.Price,
                State = orderDto.VendorId is null ? StateType.AwaitingVendor : StateType.AwaitingConfirm
            };

            await _repositoryOrder.CreateAsync(order);
            await _repositoryOrder.SaveChangesAsync();

            if (orderDto.TagIds.Any())
            {
                foreach (var tagId in orderDto.TagIds)
                {
                    await _repositoryOrderTag.CreateAsync(new OrderTag()
                    {
                        OrderId = order.Id,
                        TagId = tagId
                    });
                }
                await _repositoryOrderTag.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id, string userId)
        {
            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == id
                && order.ClientId == userId
                && order.State != StateType.InProcess && order.State != StateType.Completed);

            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            _repositoryOrder.Delete(order);
            await _repositoryOrder.SaveChangesAsync();
        }

        public async Task<OrderDto> GetOrderAsync(int id)
        {
            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == id);
            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            CommentDto commentDto = null;

            if (order.CommentCreated != null && order.CommentRating != null)
            {
                commentDto = new CommentDto()
                {
                    OrderId = order.Id,
                    Text = order.CommentText,
                    Created = (DateTime)order.CommentCreated,
                    Rating = (int)order.CommentRating
                };
            }

            var orderDto = new OrderDto
            {
                Id = order.Id,
                ClientId = order.ClientId,
                VendorId = order.VendorId,
                Title = order.Title,
                Description = order.Description,
                Price = order.Price,
                Start = order.Start,
                End = order.End,
                State = order.State,
                Comment = commentDto
            };
            return orderDto;
        }

        public async Task<IEnumerable<OrderDto>> GetIncomingOrdersAsync(string userId)
        {
            var orderDtos = new List<OrderDto>();
            var orders = await _repositoryOrder
                .GetAll()
                .AsNoTracking()
                .Where(order => order.VendorId == userId
                    && (order.State == StateType.Completed || order.State == StateType.InProcess
                    || (order.State == StateType.AwaitingConfirm && (order.End == null || order.End >= DateTime.Now.Date))))
                .ToListAsync();

            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    CommentDto commentDto = null;

                    if (order.CommentCreated != null && order.CommentRating != null)
                    {
                        commentDto = new CommentDto()
                        {
                            OrderId = order.Id,
                            Text = order.CommentText,
                            Created = (DateTime)order.CommentCreated,
                            Rating = (int)order.CommentRating
                        };
                    }
                    orderDtos.Add(new OrderDto
                    {
                        Id = order.Id,
                        Title = order.Title,
                        Description = order.Description,
                        Start = order.Start,
                        End = order.End,
                        State = order.State,
                        ClientId = order.ClientId,
                        VendorId = order.VendorId,
                        Price = order.Price,
                        Comment = commentDto
                    });
                }
            }

            return orderDtos;
        }

        public async Task<IEnumerable<OrderDto>> GetOutgoingOrdersAsync(string userId)
        {
            var orderDtos = new List<OrderDto>();
            var orders = await _repositoryOrder
                .GetAll()
                .AsNoTracking()
                .Where(order => order.ClientId == userId)
                .ToListAsync();

            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    CommentDto commentDto = null;

                    if (order.CommentCreated != null && order.CommentRating != null)
                    {
                        commentDto = new CommentDto()
                        {
                            OrderId = order.Id,
                            Text = order.CommentText,
                            Created = (DateTime)order.CommentCreated,
                            Rating = (int)order.CommentRating
                        };
                    }
                    else if ((order.State == StateType.AwaitingConfirm || order.State == StateType.AwaitingVendor)
                        && order.End < DateTime.Now.Date)
                    {
                        await UpdateOrderStateAsync(order.Id, StateType.CanceledByClient, userId);
                    }

                    orderDtos.Add(new OrderDto
                    {
                        Id = order.Id,
                        Title = order.Title,
                        Description = order.Description,
                        Start = order.Start,
                        End = order.End,
                        State = order.State,
                        ClientId = order.ClientId,
                        VendorId = order.VendorId,
                        Price = order.Price,
                        Comment = commentDto
                    });
                }
            }

            return orderDtos;
        }

        public async System.Threading.Tasks.Task UpdateOrderAsync(OrderDto orderDto, string userId)
        {
            orderDto = orderDto ?? throw new ArgumentNullException(nameof(orderDto));

            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == orderDto.Id
                && order.ClientId == userId
                && order.State != StateType.Completed
                && order.State != StateType.InProcess);

            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            static bool ValidateToUpdate(Order order, OrderDto orderDto)
            {
                bool updated = false;

                if (order.Title != orderDto.Title)
                {
                    order.Title = orderDto.Title;
                    updated = true;
                }

                if (order.Description != orderDto.Description)
                {
                    order.Description = orderDto.Description;
                    updated = true;
                }

                if (order.End != orderDto.End)
                {
                    order.End = orderDto.End;
                    updated = true;
                }

                if (order.Price != orderDto.Price)
                {
                    order.Price = orderDto.Price;
                    updated = true;
                }

                return updated;
            }

            var result = ValidateToUpdate(order, orderDto);
            if (result)
            {
                await _repositoryOrder.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task UpdateOrderStateAsync(int id, StateType state, string userId, string vendorId = null)
        {
            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == id);

            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            bool updated = false;
            switch (state)
            {
                case StateType.Completed:
                    if (order.State == StateType.InProcess
                        && order.VendorId == userId)
                    {
                        order.End = DateTime.Now;
                        updated = true;
                    }
                    break;

                case StateType.InProcess:
                    {
                        if (order.State == StateType.AwaitingVendor
                            || (order.State == StateType.AwaitingConfirm
                            && order.VendorId == userId))
                        {
                            order.VendorId = userId;
                            updated = true;
                        }
                    }
                    break;

                case StateType.CanceledByClient:
                    {
                        if (order.ClientId == userId
                            && (order.State == StateType.AwaitingConfirm
                            || order.State == StateType.AwaitingVendor
                            || order.State == StateType.InProcess))
                        {
                            updated = true;
                        }
                    }
                    break;

                case StateType.CanceledByVendor:
                    {
                        if (order.VendorId == userId
                            && (order.State == StateType.AwaitingConfirm
                            || order.State == StateType.InProcess))
                        {
                            updated = true;
                        }
                    }
                    break;

                case StateType.AwaitingConfirm:
                    {
                        if (order.ClientId == userId
                            && (order.State == StateType.CanceledByClient
                            || order.State == StateType.CanceledByVendor))
                        {
                            order.VendorId = vendorId;
                            order.Start = DateTime.Now;
                            updated = true;
                        }
                    }
                    break;

                case StateType.AwaitingVendor:
                    {
                        if (order.ClientId == userId
                            && (order.State == StateType.CanceledByClient
                            || order.State == StateType.CanceledByVendor))
                        {
                            order.VendorId = null;
                            order.Start = DateTime.Now;
                            updated = true;
                        }
                    }

                    break;
            }

            if (updated)
            {
                order.State = state;
                await _repositoryOrder.SaveChangesAsync();
                if (state != StateType.InProcess)
                {
                    var tasks = await _repositoryTask
                     .GetAll()
                     .AsNoTracking()
                     .Where(task => task.OrderId == order.Id)
                     .ToListAsync();

                    _repositoryTask.DeleteRange(tasks);
                    await _repositoryTask.SaveChangesAsync();
                }
            }
            else
            {
                throw new InvalidOperationException(ErrorResource.UnableToUpdateOrderState);
            }
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByTagsAsync(IList<int> tagIds)
        {
            var orderDtos = new List<OrderDto>();
            var orders = new List<Order>();

            if (tagIds != null && tagIds.Any())
            {
                var orderIds = await _repositoryOrderTag
                      .GetAll()
                      .AsNoTracking()
                      .Where(orderTag => tagIds.Contains(orderTag.TagId))
                      .Select(orderTag => orderTag.OrderId)
                      .ToListAsync();

                orders = await _repositoryOrder
                    .GetAll()
                    .AsNoTracking()
                    .Where(order => orderIds.Contains(order.Id)
                        && order.State == StateType.AwaitingVendor
                        && (order.End == null || order.End >= DateTime.Now.Date))
                    .ToListAsync();
            }
            else
            {
                orders = await _repositoryOrder
                    .GetAll()
                    .AsNoTracking()
                    .Where(order => order.State == StateType.AwaitingVendor
                        && (order.End == null || order.End >= DateTime.Now.Date))
                    .ToListAsync();
            }

            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    orderDtos.Add(new OrderDto
                    {
                        Id = order.Id,
                        Title = order.Title,
                        Description = order.Description,
                        Start = order.Start,
                        End = order.End,
                        State = order.State,
                        ClientId = order.ClientId,
                        VendorId = order.VendorId,
                        Price = order.Price
                    });
                }
            }
            return orderDtos;
        }

        public async System.Threading.Tasks.Task CreateOrderCommentAsync(CommentDto commentDto, string userId)
        {
            commentDto = commentDto ?? throw new ArgumentNullException(nameof(commentDto));

            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == commentDto.OrderId
                && order.ClientId == userId
                && order.State == StateType.Completed
                && order.CommentRating == null && order.CommentCreated == null);

            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            order.CommentText = commentDto.Text;
            order.CommentRating = commentDto.Rating;
            order.CommentCreated = DateTime.Now;

            await _repositoryOrder.SaveChangesAsync();
        }

        public async Task<CommentDto> GetOrderCommentAsync(int id, string userId)
        {
            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == id
                && order.State == StateType.Completed
                && order.ClientId == userId);

            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            CommentDto commentDto = null;

            if (order.CommentCreated != null && order.CommentRating != null)
            {
                commentDto = new CommentDto()
                {
                    OrderId = order.Id,
                    Text = order.CommentText,
                    Created = (DateTime)order.CommentCreated,
                    Rating = (int)order.CommentRating
                };
            }

            return commentDto;
        }

        public async System.Threading.Tasks.Task UpdateOrderCommentAsync(CommentDto commentDto, string userId)
        {
            commentDto = commentDto ?? throw new ArgumentNullException(nameof(commentDto));

            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == commentDto.OrderId
                && order.ClientId == userId
                && order.State == StateType.Completed
                && order.CommentCreated != null && order.CommentRating != null);

            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            static bool ValidateToUpdate(Order order, CommentDto commentDto)
            {
                bool updated = false;

                if (order.CommentText != commentDto.Text)
                {
                    order.CommentText = commentDto.Text;
                    updated = true;
                }

                if (order.CommentRating != commentDto.Rating)
                {
                    order.CommentRating = commentDto.Rating;
                    updated = true;
                }

                return updated;
            }

            var result = ValidateToUpdate(order, commentDto);
            if (result)
            {
                await _repositoryOrder.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task DeleteOrderCommentAsync(int id, string userId)
        {
            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == id
                && order.ClientId == userId
                && order.State == StateType.Completed
                && order.CommentCreated != null && order.CommentRating != null);

            if (order is null)
            {
                throw new KeyNotFoundException(ErrorResource.OrderNotFound);
            }

            order.CommentText = null;
            order.CommentRating = null;
            order.CommentCreated = null;
            await _repositoryOrder.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentDto>> GetUserCommentsAsync(string userId)
        {
            var commentDtos = new List<CommentDto>();

            var orders = await _repositoryOrder
                .GetAll()
                .AsNoTracking()
                .Where(order => order.VendorId == userId
                    && order.State == StateType.Completed
                    && order.CommentRating != null
                    && order.CommentCreated != null)
                .ToListAsync();

            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    commentDtos.Add(new CommentDto
                    {
                        Created = (DateTime)order.CommentCreated,
                        OrderId = order.Id,
                        AuthorId = order.ClientId,
                        Rating = (int)order.CommentRating,
                        Text = order.CommentText
                    });
                }
            }
            return commentDtos;
        }
    }
}