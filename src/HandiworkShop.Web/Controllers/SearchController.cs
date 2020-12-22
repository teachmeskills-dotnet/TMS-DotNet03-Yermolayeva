using HandiworkShop.BLL.Interfaces;
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
            return RedirectToActionPermanent("Profile", "Search");
        }

        public async Task<IActionResult> Profile(string searchString, IList<int> tagIds)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
            var profileViewModels = new List<ProfileViewModel>();

            var profiles = (await _profileManager.GetProfilesByTagsAsync(tagIds)).Where(profile => profile.UserId != userId).ToList();

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

            var allTagsViewModels = new List<TagViewModel>();
            var allTags = await _tagManager.GetAllTagsAsync();

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
            return View((
                allTagsViewModels,
                profileViewModels));
        }

        [Authorize(Roles = RolesConstants.VendorRole)]
        public async Task<IActionResult> Order(string searchString, IList<int> tagIds)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
            var orderViewModels = new List<OrderViewModel>();

            var orders = (await _orderManager.GetOrdersByTagsAsync(tagIds)).Where(order => order.ClientId != userId).ToList();

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

                    var client = await _profileManager.GetProfileAsync(order.ClientId);

                    orderViewModels.Add(new OrderViewModel()
                    {
                        Id = order.Id,
                        ClientUserName = await _accountManager.GetUserNameByIdAsync(order.ClientId),
                        ClientName = client.Name,
                        ClientAvatar = client.Avatar,
                        Description = order.Description,
                        End = order.End,
                        Price = order.Price,
                        Start = order.Start,
                        State = order.State,
                        Title = order.Title,
                        Tags = tagViewModels,
                        ClientId = order.ClientId
                    });
                }
            }

            var allTagsViewModels = new List<TagViewModel>();
            var allTags = await _tagManager.GetAllTagsAsync();

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
            return View((
                allTagsViewModels,
                orderViewModels));
        }
    }
}