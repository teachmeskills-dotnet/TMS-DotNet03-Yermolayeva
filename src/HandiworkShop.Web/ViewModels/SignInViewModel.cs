using System.ComponentModel.DataAnnotations;

namespace HandiworkShop.Web.ViewModels
{
    /// <summary>
    /// Sign in view model.
    /// </summary>
    public class SignInViewModel
    {
        /// <summary>
        /// Email.
        /// </summary>
        [Required(ErrorMessage = "Укажите имя пользователя")]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        /// <summary>
        /// Remember me.
        /// </summary>
        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// Return url.
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}