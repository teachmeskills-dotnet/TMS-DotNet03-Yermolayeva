using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.Common.Enums;
using HandiworkShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Managers
{
    public class OrderManager : IOrderManager
    {
        private readonly IRepository<Order> _repositoryOrder;
        private readonly IRepository<DAL.Entities.Task> _repositoryTask;

        public OrderManager(IRepository<Order> repositoryOrder, IRepository<DAL.Entities.Task> repositoryTask)
        {
            _repositoryOrder = repositoryOrder ?? throw new ArgumentNullException(nameof(repositoryOrder));
            _repositoryTask = repositoryTask ?? throw new ArgumentNullException(nameof(repositoryTask));
        }

        public async System.Threading.Tasks.Task CreateAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                Title = orderDto.Title,
                Description = orderDto.Description,
                Start = DateTime.Now,
                End = orderDto.End,
                ClientId = orderDto.ClientId,
                VendorId = orderDto.VendorId,
                Price = orderDto.Price,
                State = StateType.AwaitingConfirm
            };

            await _repositoryOrder.CreateAsync(order);
            await _repositoryOrder.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == id);
            if (order is null)
            {
                return;
            }

            if (!(order.Vendor is null))
            {
                return;
            }

            _repositoryOrder.Delete(order);
            await _repositoryOrder.SaveChangesAsync();
        }

        public async Task<OrderDto> GetOrderAsync(int id)
        {
            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == id);
            if (order is null)
            {
                return null;
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
                State = order.State
            };
            return orderDto;
        }

        public async Task<IEnumerable<OrderDto>> GetIncomingOrdersAsync(string userId)
        {
            var orderDtos = new List<OrderDto>();
            var orders = await _repositoryOrder
                .GetAll()
                .AsNoTracking()
                .Where(order => order.VendorId == userId)
                .ToListAsync();

            if (!orders.Any())
            {
                return orderDtos;
            }

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

            if (!orders.Any())
            {
                return orderDtos;
            }

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

            return orderDtos;
        }

        public async System.Threading.Tasks.Task UpdateOrderAsync(OrderDto orderDto)
        {
            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == orderDto.Id);
            if (order is null)
            {
                return;
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

                return updated;
            }

            var result = ValidateToUpdate(order, orderDto);
            if (result)
            {
                await _repositoryOrder.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task UpdateOrderStatusAsync(int id, StateType state)
        {
            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == id);
            if (order is null || order.State == state)
            {
                return;
            }

            order.State = state;
            await _repositoryOrder.SaveChangesAsync();

            if (state != StateType.InProcess)
            {
                var tasks = await _repositoryTask
                 .GetAll()
                 .AsNoTracking()
                 .Where(task => task.OrderId == order.Id)
                 .ToListAsync();

                foreach (var task in tasks)
                {
                    _repositoryTask.Delete(task);
                }
                await _repositoryTask.SaveChangesAsync();
            }
        }
    }
}
