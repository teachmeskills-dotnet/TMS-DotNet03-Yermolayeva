using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.Web.ViewModels
{
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Old password.
        /// </summary>
        [Required(ErrorMessage = "Введите старый пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        /// <summary>
        /// New password.
        /// </summary>
        [Required(ErrorMessage = "Введите новый пароль")]
        [MinLength(8, ErrorMessage = "Пароль слишком короткий (минимум 8 символов)")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        /// <summary>
        /// New password confirm.
        /// </summary>
        [Required(ErrorMessage = "Подтвердите новый пароль")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string NewPasswordConfirm { get; set; }
    }
}
