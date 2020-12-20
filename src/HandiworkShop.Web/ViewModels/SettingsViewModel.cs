using HandiworkShop.Common.Constants;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HandiworkShop.Web.ViewModels
{
    /// <summary>
    /// Settings view model.
    /// </summary>
    public class SettingsViewModel
    {
        /// <summary>
        /// Profile identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Is Vendor.
        /// </summary>
        [Display(Name = "Продавец?")]
        public bool IsVendor { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Введите имя")]
        [MaxLength(ConfigurationConstants.StandartLenghtForStringField, ErrorMessage = "Слишком длинное имя")]
        public string Name { get; set; }

        /// <summary>
        /// Info.
        /// </summary>
        [Display(Name = "Информация")]
        [DataType(DataType.MultilineText)]
        public string Info { get; set; }

        /// <summary>
        /// Avatar.
        /// </summary>
        public byte[] Avatar { get; set; }

        /// <summary>
        /// New avatar.
        /// </summary>
        [Display(Name = "Изменить аватар")]
        public IFormFile NewAvatar { get; set; }

        /// <summary>
        /// Tag identifiers.
        /// </summary>
        public int[] TagIds { get; set; }

        /// <summary>
        /// All tags.
        /// </summary>
        public ICollection<TagViewModel> AllTags { get; set; }
    }
}