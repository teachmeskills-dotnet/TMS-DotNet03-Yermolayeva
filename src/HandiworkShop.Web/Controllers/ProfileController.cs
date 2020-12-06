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
    public class ProfileController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly IProfileManager _profileManager;
        private readonly IOrderManager _orderManager;
        private readonly ITagManager _tagManager;


        public ProfileController(
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

        [Route("profile/{userName?}")]
        [HttpGet]
        public async Task<IActionResult> Index(string userName)
        {
            userName = userName ?? User.Identity.Name;
            var userId = await _accountManager.GetUserIdByNameAsync(userName);

            var profile = await _profileManager.GetProfileAsync(userId);
            var comments = await _orderManager.GetUserCommentsAsync(userId);
            var tags = await _tagManager.GetUserTagsAsync(userId);

            var commentViewModels = new List<CommentViewModel>();
            var tagViewModels = new List<TagViewModel>();
            double? rating = null;

            if (comments.Any())
            {
                foreach (var comment in comments)
                {
                    var author = await _profileManager.GetProfileAsync(comment.AuthorId);
                    commentViewModels.Add(new CommentViewModel
                    {
                        OrderId = comment.OrderId,
                        Created = comment.Created,
                        Rating = comment.Rating,
                        Text = comment.Text,
                        AuthorName = author.Name,
                        AuthorAvatar = author.Avatar,
                        AuthorUserName = await _accountManager.GetUserNameByIdAsync(author.UserId)
                    });
                }
                rating = commentViewModels.Select(comment => comment.Rating).Average();
            }
            if (tags.Any())
            {
                foreach(var tag in tags)
                {
                    tagViewModels.Add(new TagViewModel
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
                }
            }

            var ordersCompleted = (await _orderManager.GetIncomingOrdersAsync(profile.UserId))
                .Where(order => order.State == StateType.Completed).Count();

            var profileViewModel = new ProfileViewModel()
            {
                Id = profile.Id,
                UserId = profile.UserId,
                UserName = userName,
                Avatar = profile.Avatar,
                Created = profile.Created,
                IsVendor = profile.IsVendor,
                Info = profile.Info,
                Name = profile.Name,
                Rating = rating,
                Comments = commentViewModels,
                Tags = tagViewModels,
                OrdersCompleted = ordersCompleted
            };

            return View(profileViewModel);
        }
    }
}
