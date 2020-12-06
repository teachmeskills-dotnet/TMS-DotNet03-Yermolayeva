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
    public class SearchController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly IProfileManager _profileManager;
        private readonly IOrderManager _orderManager;
        private readonly ITagManager _tagManager;


        public SearchController(
            IAccountManager accountManager,
            IProfileManager profileManager,
            IOrderManager orderManager,
            ITagManager tagManager)
        {
            _accountManager = accountManager ?? throw new ArgumentNullException(nameof(accountManager));
            _profileManager = profileManager ?? throw new ArgumentNullException(nameof(profileManager));
            _orderManager = orderManager ?? throw new ArgumentNullException(nameof(orderManager));
            _tagManager = tagManager ?? throw new ArgumentNullException(nameof(tagManager));

        }
        public IActionResult Index()
        {
            return RedirectToActionPermanent("OutgoingOrders", "Orders");
        }
        public async Task<IActionResult> SearchProfile(string searchString, IList<int> tagIds)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
            var profileViewModels = new List<ProfileViewModel>();

            var profiles = (List<ProfileDto>)await _profileManager.GetProfilesByTagsAsync(tagIds);
            profiles = profiles.Where(profile => profile.UserId != userId).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                profiles = profiles.Where(profile => profile.Name.Contains(searchString)).ToList();
            }

            if (profiles.Any())
            {
                foreach (var profile in profiles)
                {
                    var tags = await _tagManager.GetUserTagsAsync(profile.UserId);
                    var tagViewModels = new List<TagViewModel>();

                    if (tags.Any())
                    {
                        foreach (var tag in tags)
                        {
                            tagViewModels.Add(new TagViewModel
                            {
                                Id = tag.Id,
                                Name = tag.Name
                            });
                        }
                    }

                    double? rating = null;
                    var comments = await _orderManager.GetUserCommentsAsync(profile.UserId);
                    if (comments.Any())
                    {
                        rating = comments.Select(comment => comment.Rating).Average();
                    }

                    var ordersCompleted = (await _orderManager.GetIncomingOrdersAsync(profile.UserId))
                        .Where(order => order.State == StateType.Completed).Count();

                    profileViewModels.Add(new ProfileViewModel()
                    {
                        Id = profile.Id,
                        Name = profile.Name,
                        Avatar = profile.Avatar,
                        Created = profile.Created,
                        Info = profile.Info,
                        UserName = await _accountManager.GetUserNameByIdAsync(profile.UserId),
                        IsVendor = profile.IsVendor,
                        UserId = profile.UserId,
                        Rating = rating,
                        Tags = tagViewModels,
                        OrdersCompleted = ordersCompleted
                    });
                }

            }

            var allTags = await _tagManager.GetAllTagsAsync();
            var allTagsViewModels = new List<TagViewModel>();

            foreach (var tag in allTags)
            {
                allTagsViewModels.Add(new TagViewModel()
                {
                    Id = tag.Id,
                    Name = tag.Name
                });
            }
            return View((
                allTagsViewModels,
                profileViewModels));
        }

        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> SearchOrder(string searchString, IList<int> tagIds)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
            var orderViewModels = new List<OrderViewModel>();

            var orders = (List<OrderDto>)await _orderManager.GetOrdersByTagsAsync(tagIds);
            orders = orders.Where(order => order.ClientId != userId).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(order => order.Title.Contains(searchString)).ToList();
            }

            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    var tags = await _tagManager.GetOrderTagsAsync(order.Id);
                    var tagViewModels = new List<TagViewModel>();

                    if (tags.Any())
                    {
                        foreach (var tag in tags)
                        {
                            tagViewModels.Add(new TagViewModel
                            {
                                Id = tag.Id,
                                Name = tag.Name
                            });
                        }
                    }

                    orderViewModels.Add(new OrderViewModel()
                    {
                        Id = order.Id,
                        ClientUserName = await _accountManager.GetUserNameByIdAsync(order.ClientId),
                        ClientAvatar = (await _profileManager.GetProfileAsync(order.ClientId)).Avatar,
                        Description = order.Description,
                        End = order.End,
                        Price = order.Price,
                        Start = order.Start,
                        State = order.State,
                        Title = order.Title,
                        VendorId = order.VendorId,
                        Tags = tagViewModels
                    });
                }

            }

            var allTags = await _tagManager.GetAllTagsAsync();
            var allTagsViewModels = new List<TagViewModel>();

            foreach(var tag in allTags)
            {
                allTagsViewModels.Add(new TagViewModel()
                {
                    Id = tag.Id,
                    Name = tag.Name
                });
            }
            return View((
                allTagsViewModels,
                orderViewModels));
        }
    }
}