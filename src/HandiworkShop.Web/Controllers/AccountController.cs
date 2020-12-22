using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.DAL.Entities;
using HandiworkShop.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly IProfileManager _profileManager;
        private readonly ITagManager _tagManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            IAccountManager accountManager,
            IProfileManager profileManager,
            ITagManager tagManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _accountManager = accountManager ?? throw new ArgumentNullException(nameof(accountManager));
            _profileManager = profileManager ?? throw new ArgumentNullException(nameof(profileManager));
            _tagManager = tagManager ?? throw new ArgumentNullException(nameof(tagManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountManager.SignUpAsync(model.Email, model.Username, model.Password, model.IsVendor);
                if (result.Item1.Succeeded)
                {
                    await _signInManager.SignInAsync(result.Item2, false);
                    return RedirectToAction("Index", "Profile");
                }

                foreach (var error in result.Item1.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult SignIn(string returnUrl = null)
        {
            var signInViewModel = new SignInViewModel()
            {
                ReturnUrl = returnUrl
            };
            return View(signInViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Profile");
                }

                ModelState.AddModelError(string.Empty, "Неверный логин и (или) пароль.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
                var result = await _accountManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToAction("Settings", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
            var profile = await _profileManager.GetProfileAsync(userId);
            var tagIds = (await _tagManager.GetUserTagsAsync(userId)).Select(tag => tag.Id);
            var allTags = (await _tagManager.GetAllTagsAsync()).ToList();

            var tagViewModels = new List<TagViewModel>();

            if (allTags.Any())
            {
                foreach (var tag in allTags)
                {
                    tagViewModels.Add(new TagViewModel()
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
                }
            }

            var settingsViewModel = new SettingsViewModel()
            {
                Id = profile.Id,
                Name = profile.Name,
                Info = profile.Info,
                IsVendor = profile.IsVendor,
                UserId = profile.UserId,
                TagIds = tagIds.ToArray(),
                AllTags = tagViewModels,
                Avatar = profile.Avatar
            };

            return View(settingsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(SettingsViewModel settingsViewModel)
        {
            var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);
            if (ModelState.IsValid)
            {
                var profileDto = new ProfileDto()
                {
                    Id = settingsViewModel.Id,
                    UserId = settingsViewModel.UserId,
                    Info = settingsViewModel.Info,
                    Name = settingsViewModel.Name,
                    IsVendor = settingsViewModel.IsVendor
                };

                if (settingsViewModel.NewAvatar != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(settingsViewModel.NewAvatar.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)settingsViewModel.NewAvatar.Length);
                    }
                    profileDto.Avatar = imageData;
                }

                await _profileManager.UpdateProfileAsync(profileDto, userId);
                await _tagManager.UpdateUserTagsAsync(userId, settingsViewModel.TagIds ?? Array.Empty<int>());

                return RedirectToAction("Settings", "Account");
            }
            var allTags = (await _tagManager.GetAllTagsAsync()).ToList();

            var tagViewModels = new List<TagViewModel>();

            if (allTags.Any())
            {
                foreach (var tag in allTags)
                {
                    tagViewModels.Add(new TagViewModel()
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
                }
            }
            settingsViewModel.AllTags = tagViewModels;

            settingsViewModel.Avatar = (await _profileManager.GetProfileAsync(userId)).Avatar;

            return View(settingsViewModel);
        }
    }
}