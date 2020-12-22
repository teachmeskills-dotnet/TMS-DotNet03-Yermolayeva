using System.ComponentModel.DataAnnotations;

namespace HandiworkShop.Web.ViewModels
{
    /// <summary>
    /// Sign up view model.
    /// </summary>
    public class SignUpViewModel
    {
        /// <summary>
        /// Email.
        /// </summary>
        [Required(ErrorMessage = "Укажите Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Username.
        /// </summary>
        [Required(ErrorMessage = "Укажите имя пользователя")]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }

        /// <summary>
        /// Is vendor.
        /// </summary>
        [Display(Name = "Продавец")]
        public bool IsVendor { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        /// <summary>
        /// Password confirm.
        /// </summary>
        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}