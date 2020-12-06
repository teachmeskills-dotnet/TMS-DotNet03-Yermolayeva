using HandiworkShop.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.Web.ViewModels
{
    public class SettingsViewModel
    {
        public int Id { get; set; }
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
        [MaxLength(ConfigurationConstants.StandartLenghtForStringField, ErrorMessage ="Слишком длинное имя")]
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

        [Display(Name = "Аватар")]
        public byte[] Avatar { get; set; }

        /// <summary>
        /// Tag ids.
        /// </summary>
        public int[] TagIds { get; set; }

        /// <summary>
        /// All tags.
        /// </summary>
        public IList<TagViewModel> AllTags { get; set; }
    }
}
