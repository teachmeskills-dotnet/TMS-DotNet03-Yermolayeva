using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.Common.Constants;
using HandiworkShop.Common.Enums;
using HandiworkShop.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.Web.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly IProfileManager _profileManager;
        private readonly IOrderManager _orderManager;
        private readonly ITaskManager _taskManager;
        private readonly ITagManager _tagManager;

        public OrdersController(
            IAccountManager accountManager,
            IProfileManager profileManager,
            IOrderManager orderManager,
            ITaskManager taskManager,
            ITagManager tagManager)
        {
            _accountManager = accountManager ?? throw new ArgumentNullException(nameof(accountManager));
            _profileManager = profileManager ?? throw new ArgumentNullException(nameof(profileManager));
            _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            _orderManager = orderManager ?? throw new ArgumentNullException(nameof(orderManager));
            _tagManager = tagManager ?? throw new ArgumentNullException(nameof(tagManager));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToActionPermanent("Outgoing", "Orders");
        }

        [HttpGet]
        public async Task<IActionResult> Outgoing()
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
            var outgoingOrders = await _orderManager.GetOutgoingOrdersAsync(userId);

            var orderViewModels = new List<OrderViewModel>();

            if (outgoingOrders.Any())
            {
                foreach (var order in outgoingOrders)
                {
                    var tags = await _tagManager.GetOrderTagsAsync(order.Id);
                    var tagViewModels = new List<TagViewModel>();

                    if (tags.Any())
                    {
                        foreach (var tag in tags)
                        {
                            tagViewModels.Add(new TagViewModel()
                            {
                                Id = tag.Id,
                                Name = tag.Name
                            });
                        }
                    }
                    var vendor = order.VendorId is null ? null : (await _profileManager.GetProfileAsync(order.VendorId));
                    orderViewModels.Add(new OrderViewModel()
                    {
                        Id = order.Id,
                        Price = order.Price,
                        Description = order.Description,
                        Title = order.Title,
                        Start = order.Start,
                        End = order.End,
                        State = order.State,
                        ClientId = order.ClientId,
                        VendorId = order.VendorId,
                        VendorName = vendor is null ? null : vendor.Name,
                        VendorUserName = order.VendorId is null ? null : await _accountManager.GetUserNameByIdAsync(order.VendorId),
                        VendorAvatar = vendor is null ? null : vendor.Avatar,
                        Comment = order.Comment == null ? null : new CommentViewModel()
                        {
                            OrderId = order.Id,
                            Created = order.Comment.Created,
                            Rating = order.Comment.Rating,
                            Text = order.Comment.Text
                        },
                        Tags = tagViewModels
                    });
                }
            }

            return View(orderViewModels);
        }

        [Authorize(Roles = RolesConstants.VendorRole)]
        [HttpGet]
        public async Task<IActionResult> Incoming()
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
            var incomingOrders = await _orderManager.GetIncomingOrdersAsync(userId);

            var orderViewModels = new List<OrderViewModel>();

            if (incomingOrders.Any())
            {
                foreach (var order in incomingOrders)
                {
                    var tags = await _tagManager.GetOrderTagsAsync(order.Id);
                    var tagViewModels = new List<TagViewModel>();

                    if (tags.Any())
                    {
                        foreach (var tag in tags)
                        {
                            tagViewModels.Add(new TagViewModel()
                            {
                                Id = tag.Id,
                                Name = tag.Name
                            });
                        }
                    }

                    var tasks = await _taskManager.GetOrderTasksAsync(order.Id, userId);
                    var taskViewModels = new List<TaskViewModel>();

                    if (tasks.Any())
                    {
                        foreach (var task in tasks)
                        {
                            taskViewModels.Add(new TaskViewModel()
                            {
                                Id = task.Id,
                                Description = task.Description,
                                End = task.End,
                                Start = task.Start,
                                State = task.State,
                                Title = task.Title
                            });
                        }
                    }
                    var client = await _profileManager.GetProfileAsync(order.ClientId);

                    orderViewModels.Add(new OrderViewModel()
                    {
                        Id = order.Id,
                        Price = order.Price,
                        ClientUserName = await _accountManager.GetUserNameByIdAsync(order.ClientId),
                        ClientName = client.Name,
                        Comment = order.Comment == null ? null : new CommentViewModel()
                        {
                            OrderId = order.Id,
                            Created = order.Comment.Created,
                            Rating = order.Comment.Rating,
                            Text = order.Comment.Text
                        },
                        ClientAvatar = client.Avatar,
                        ClientId = order.ClientId,
                        VendorId = order.VendorId,
                        Description = order.Description,
                        Title = order.Title,
                        Start = order.Start,
                        End = order.End,
                        State = order.State,
                        Tags = tagViewModels,
                        Tasks = taskViewModels
                    });
                }
            }

            return View(orderViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> EditOrder(int orderId)
        {
            var order = await _orderManager.GetOrderAsync(orderId);
            var tags = await _tagManager.GetOrderTagsAsync(orderId);
            var tagIds = tags.Select(tag => tag.Id);

            var allTags = await _tagManager.GetAllTagsAsync();
            var allTagsViewModels = new List<TagViewModel>();

            if (allTags.Any())
            {
                foreach (var tag in allTags)
                {
                    allTagsViewModels.Add(new TagViewModel()
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
                }
            }

            var orderEditViewModel = new OrderEditViewModel()
            {
                Id = order.Id,
                ClientId = order.ClientId,
                Description = order.Description,
                End = order.End,
                Price = order.Price,
                Title = order.Title,
                VendorId = order.VendorId,
                TagIds = tagIds.ToArray(),
                AllTags = allTagsViewModels
            };
            return View(orderEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrder(OrderEditViewModel orderEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
                var orderDto = new OrderDto()
                {
                    Id = orderEditViewModel.Id,
                    Title = orderEditViewModel.Title,
                    Description = orderEditViewModel.Description,
                    Price = orderEditViewModel.Price,
                    ClientId = userId,
                    VendorId = orderEditViewModel.VendorId,
                    End = orderEditViewModel.End
                };
                await _orderManager.UpdateOrderAsync(orderDto, userId);
                await _tagManager.UpdateOrderTagsAsync(orderDto.Id, orderEditViewModel.TagIds, userId);
                return RedirectToAction("Outgoing", "Orders");
            }
            var allTags = await _tagManager.GetAllTagsAsync();
            var allTagsViewModels = new List<TagViewModel>();

            if (allTags.Any())
            {
                foreach (var tag in allTags)
                {
                    allTagsViewModels.Add(new TagViewModel()
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
                }
            }
            orderEditViewModel.AllTags = allTagsViewModels;
            return View(orderEditViewModel);
        }

        [HttpGet]
        [Authorize(Roles = RolesConstants.VendorRole)]
        public async Task<IActionResult> EditTask(int taskId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
            var task = await _taskManager.GetTaskAsync(taskId, userId);

            var taskEditViewModel = new TaskViewModel()
            {
                Id = task.Id,
                OrderId = task.OrderId,
                Description = task.Description,
                Start = task.Start,
                End = task.End,
                Title = task.Title,
            };
            return View(taskEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RolesConstants.VendorRole)]
        public async Task<IActionResult> EditTask(TaskViewModel taskEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
                var taskDto = new TaskDto()
                {
                    Id = taskEditViewModel.Id,
                    OrderId = taskEditViewModel.OrderId,
                    Start = taskEditViewModel.Start,
                    Description = taskEditViewModel.Description,
                    Title = taskEditViewModel.Title,
                    End = taskEditViewModel.End
                };
                await _taskManager.UpdateTaskAsync(taskDto, userId);
                return RedirectToAction("Incoming", "Orders");
            }
            return View(taskEditViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditComment(int orderId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
            var comment = await _orderManager.GetOrderCommentAsync(orderId, userId);

            var commentViewModel = new CommentViewModel()
            {
                OrderId = comment.OrderId,
                Rating = comment.Rating,
                Text = comment.Text
            };
            return View(commentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(CommentViewModel commentViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
                var commentDto = new CommentDto()
                {
                    OrderId = commentViewModel.OrderId,
                    Rating = commentViewModel.Rating,
                    Text = commentViewModel.Text
                };
                await _orderManager.UpdateOrderCommentAsync(commentDto, userId);
                return RedirectToAction("Outgoing", "Orders");
            }
            return View(commentViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder(string vendorId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            var allTags = await _tagManager.GetAllTagsAsync();
            var allTagsViewModels = new List<TagViewModel>();

            if (allTags.Any())
            {
                foreach (var tag in allTags)
                {
                    allTagsViewModels.Add(new TagViewModel()
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
                }
            }

            var orderEditViewModel = new OrderEditViewModel()
            {
                VendorId = vendorId,
                ClientId = userId,
                AllTags = allTagsViewModels
            };
            return View(orderEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(OrderEditViewModel orderEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
                var orderDto = new OrderDto()
                {
                    Title = orderEditViewModel.Title,
                    Description = orderEditViewModel.Description,
                    Price = orderEditViewModel.Price,
                    ClientId = userId,
                    VendorId = orderEditViewModel.VendorId,
                    TagIds = orderEditViewModel.TagIds ?? Array.Empty<int>(),
                    End = orderEditViewModel.End
                };
                await _orderManager.CreateAsync(orderDto);
                return RedirectToAction("Outgoing", "Orders");
            }
            var allTags = await _tagManager.GetAllTagsAsync();
            var allTagsViewModels = new List<TagViewModel>();

            if (allTags.Any())
            {
                foreach (var tag in allTags)
                {
                    allTagsViewModels.Add(new TagViewModel()
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
                }
            }
            orderEditViewModel.AllTags = allTagsViewModels;
            return View(orderEditViewModel);
        }

        [HttpGet]
        [Authorize(Roles = RolesConstants.VendorRole)]
        public IActionResult CreateTask(int orderId)
        {
            var taskEditViewModel = new TaskViewModel()
            {
                OrderId = orderId
            };
            return View(taskEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RolesConstants.VendorRole)]
        public async Task<IActionResult> CreateTask(TaskViewModel taskEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
                var taskDto = new TaskDto()
                {
                    OrderId = taskEditViewModel.OrderId,
                    Start = taskEditViewModel.Start,
                    Description = taskEditViewModel.Description,
                    Title = taskEditViewModel.Title,
                    End = taskEditViewModel.End
                };
                await _taskManager.CreateAsync(taskDto, userId);
                return RedirectToAction("Incoming", "Orders");
            }
            return View(taskEditViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateComment(int orderId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            var commentViewModel = new CommentViewModel()
            {
                OrderId = orderId
            };
            return View(commentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(CommentViewModel commentViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
                var commentDto = new CommentDto()
                {
                    OrderId = commentViewModel.OrderId,
                    Rating = commentViewModel.Rating,
                    Text = commentViewModel.Text
                };
                await _orderManager.CreateOrderCommentAsync(commentDto, userId);
                return RedirectToAction("Outgoing", "Orders");
            }
            return View(commentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int orderId, string returnUrl)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _orderManager.DeleteOrderCommentAsync(orderId, userId);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Outgoing", "Orders");
        }

        [Authorize(Roles = RolesConstants.VendorRole)]
        [HttpPost]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _taskManager.DeleteAsync(taskId, userId);

            return RedirectToAction("Incoming", "Orders");
        }

        [Authorize(Roles = RolesConstants.VendorRole)]
        [HttpPost]
        public async Task<IActionResult> CompleteTask(int taskId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _taskManager.UpdateTaskStateAsync(taskId, userId);

            return RedirectToAction("Incoming", "Orders");
        }

        [Authorize(Roles = RolesConstants.VendorRole)]
        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _orderManager.UpdateOrderStateAsync(orderId, StateType.Completed, userId);

            return RedirectToAction("Incoming", "Orders");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _orderManager.DeleteAsync(orderId, userId);

            return RedirectToAction("Outgoing", "Orders");
        }

        [Authorize(Roles = RolesConstants.VendorRole)]
        [HttpPost]
        public async Task<IActionResult> AcceptOrder(int orderId, string returnUrl)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _orderManager.UpdateOrderStateAsync(orderId, StateType.InProcess, userId);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Incoming", "Orders");
        }

        [HttpPost]
        public async Task<IActionResult> CancelOutgoingOrder(int orderId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _orderManager.UpdateOrderStateAsync(orderId, StateType.CanceledByClient, userId);

            return RedirectToAction("Outgoing", "Orders");
        }

        [Authorize(Roles = RolesConstants.VendorRole)]
        [HttpPost]
        public async Task<IActionResult> CancelIncomingOrder(int orderId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _orderManager.UpdateOrderStateAsync(orderId, StateType.CanceledByVendor, userId);

            return RedirectToAction("Incoming", "Orders");
        }
    }
}