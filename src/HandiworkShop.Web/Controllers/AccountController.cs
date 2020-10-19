using System;
using System.Threading.Tasks;
using HandiworkShop.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HandiworkShop.BLL.Interfaces;
using HandiworkShop.DAL.Entities;

namespace HandiworkShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountManager accountManager, SignInManager<ApplicationUser> signInManager)
        {
            _accountManager = accountManager ?? throw new ArgumentNullException(nameof(accountManager));
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
                var result = await _accountManager.SignUpAsync(model.Email, model.Username, model.Password);

                if (result.Item1.Succeeded)
                {
                    await _signInManager.SignInAsync(result.Item2, false);
                    return RedirectToAction("Index", "Home");
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
                    return RedirectToAction("Index", "Home");
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
    }
}
