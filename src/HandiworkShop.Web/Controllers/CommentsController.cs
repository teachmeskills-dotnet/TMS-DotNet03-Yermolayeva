using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.Common.Enums;
using HandiworkShop.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HandiworkShop.Web.Controllers
{
    public class CommentsController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly IOrderManager _orderManager;

        public CommentsController(
            IAccountManager accountManager,
            IOrderManager orderManager)
        {
            _accountManager = accountManager ?? throw new ArgumentNullException(nameof(accountManager));
            _orderManager = orderManager ?? throw new ArgumentNullException(nameof(orderManager));
        }

        //[HttpGet]
        //public async Task<IActionResult> EditComment(int commentId)
        //{
        //    var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);

        //    if (comment.AuthorId == userId)
        //    {
        //        var commentViewModel = new CommentViewModel()
        //        {
        //            Id = comment.Id,
        //            Created = comment.Created,
        //            AuthorId = comment.AuthorId,
        //            ProfileId = comment.ProfileId,
        //            Rating = comment.Rating,
        //            Text = comment.Text
        //        };
        //        return View(commentViewModel);
        //    }
        //    else
        //    {
        //        throw new Exception();
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditComment(CommentViewModel commentViewModel)
        //{
        //    var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
        //    commentViewModel = commentViewModel ?? throw new ArgumentNullException(nameof(commentViewModel));

        //    if (ModelState.IsValid)
        //    {
        //        var commentDto = new CommentDto
        //        {
        //            Id = commentViewModel.Id,
        //            Text = commentViewModel.Text,
        //            AuthorId = commentViewModel.AuthorId,
        //            Created = commentViewModel.Created,
        //            ProfileId = commentViewModel.ProfileId,
        //            Rating = commentViewModel.Rating
        //        };

        //        await _commentManager.UpdateCommentAsync(commentDto, userId);
        //    }
        //    return View(commentViewModel);
        //}

        //[HttpGet]
        //public async Task<IActionResult> CreateCommment(int orderId)
        //{
        //    var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
        //    var order = await _orderManager.GetOrderAsync(orderId);

        //    if (order.ClientId == userId && order.State == StateType.Completed)
        //    {
        //        var commentViewModel = new CommentViewModel()
        //        {
        //            AuthorId = userId,
        //            ProfileId = order.VendorId
                    
        //        };
        //        return View(commentViewModel);
        //    }
        //    else
        //    {
        //        throw new Exception();
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateComment(CommentViewModel commentViewModel)
        //{
        //    commentViewModel = commentViewModel ?? throw new ArgumentNullException(nameof(commentViewModel));

        //    if (ModelState.IsValid)
        //    {
        //        var commentDto = new CommentDto
        //        {
        //            Text = commentViewModel.Text,
        //            AuthorId = commentViewModel.AuthorId,
        //            ProfileId = commentViewModel.ProfileId,
        //            Rating = commentViewModel.Rating
        //        };

        //        await _commentManager.CreateAsync(commentDto);
        //    }
        //    return View(commentViewModel);
        //}
    }
}
