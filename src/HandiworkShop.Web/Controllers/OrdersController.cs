using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.Common.Enums;
using HandiworkShop.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return RedirectToActionPermanent("OutgoingOrders", "Orders");
        }

        [HttpGet]
        public async Task<IActionResult> OutgoingOrders()
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

                    orderViewModels.Add(new OrderViewModel()
                    {
                        Id = order.Id,
                        Price = order.Price,
                        ClientId = order.ClientId,
                        ClientUserName = await _accountManager.GetUserNameByIdAsync(order.ClientId),
                        ClientAvatar = (await _profileManager.GetProfileAsync(order.ClientId)).Avatar,
                        Description = order.Description,
                        Title = order.Title,
                        Start = order.Start,
                        End = order.End,
                        State = order.State,
                        VendorId = order.VendorId,
                        VendorUserName = await _accountManager.GetUserNameByIdAsync(order.VendorId),
                        VendorAvatar = (await _profileManager.GetProfileAsync(order.VendorId)).Avatar,
                        HasComment = order.Comment != null,
                        Tags = tagViewModels
                    });
                }
            }

            return View(orderViewModels);
        }

        [Authorize(Roles = "Vendor")]
        [HttpGet]
        public async Task<IActionResult> IncomingOrders()
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

                    orderViewModels.Add(new OrderViewModel()
                    {
                        Id = order.Id,
                        Price = order.Price,
                        ClientUserName = await _accountManager.GetUserNameByIdAsync(order.ClientId),
                        ClientAvatar = (await _profileManager.GetProfileAsync(order.ClientId)).Avatar,
                        ClientId = order.ClientId,
                        Description = order.Description,
                        Title = order.Title,
                        Start = order.Start,
                        End = order.End,
                        State = order.State,
                        VendorId = order.VendorId,
                        VendorUserName = await _accountManager.GetUserNameByIdAsync(order.VendorId),
                        VendorAvatar = (await _profileManager.GetProfileAsync(order.VendorId)).Avatar,
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
                return RedirectToAction("OutgoingOrders", "Orders");
            }
            return View(orderEditViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> EditTask(int taskId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
            var task = await _taskManager.GetTaskAsync(taskId, userId);

            var taskEditViewModel = new TaskEditViewModel()
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
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> EditTask(TaskEditViewModel taskEditViewModel)
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
                return RedirectToAction("IncomingOrders", "Orders");
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
                return RedirectToAction("OutgoingOrders", "Orders");
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
                    TagIds = orderEditViewModel.TagIds ?? new int[0],
                    End = orderEditViewModel.End
                };
                await _orderManager.CreateAsync(orderDto);
                return RedirectToAction("OutgoingOrders", "Orders");
            }
            return View(orderEditViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Vendor")]
        public IActionResult CreateTask(int orderId)
        {
            var taskEditViewModel = new TaskEditViewModel()
            {
                OrderId = orderId
            };
            return View(taskEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> CreateTask(TaskEditViewModel taskEditViewModel)
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
                return RedirectToAction("IncomingOrders", "Orders");
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
                return RedirectToAction("OutgoingOrders", "Orders");
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
            return RedirectToAction("OutgoingOrders", "Orders");
        }

        [Authorize(Roles = "Vendor")]
        [HttpPost]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _taskManager.DeleteAsync(taskId, userId);

            return RedirectToAction("IncomingOrders", "Orders");
        }

        [Authorize(Roles = "Vendor")]
        [HttpPost]
        public async Task<IActionResult> CompleteTask(int taskId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _taskManager.UpdateTaskStateAsync(taskId, userId);

            return RedirectToAction("IncomingOrders", "Orders");
        }

        [Authorize(Roles = "Vendor")]
        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _orderManager.UpdateOrderStateAsync(orderId, StateType.Completed, userId);

            return RedirectToAction("IncomingOrders", "Orders");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _orderManager.DeleteAsync(orderId, userId);

            return RedirectToAction("OutgoingOrders", "Orders");
        }

        [Authorize(Roles = "Vendor")]
        [HttpPost]
        public async Task<IActionResult> AcceptOrder(int orderId, string returnUrl)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _orderManager.UpdateOrderStateAsync(orderId, StateType.InProcess, userId);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Orders");
        }

        [HttpPost]
        public async Task<IActionResult> CancelOutgoingOrder(int orderId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _orderManager.UpdateOrderStateAsync(orderId, StateType.CanceledByClient, userId);

            return RedirectToAction("OutGoingOrders", "Orders");
        }

        [Authorize(Roles = "Vendor")]
        [HttpPost]
        public async Task<IActionResult> CancelIncomingOrder(int orderId)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

            await _orderManager.UpdateOrderStateAsync(orderId, StateType.CanceledByVendor, userId);

            return RedirectToAction("IncomingOrders", "Orders");
        }
    }
} 