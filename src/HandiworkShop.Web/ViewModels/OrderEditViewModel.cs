using HandiworkShop.Common.Constants;
using HandiworkShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.Web.ViewModels
{
    public class OrderEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название заказа")]
        [Required(ErrorMessage = "Введите название заказа")]
        [MaxLength(ConfigurationConstants.StandartLenghtForStringField, ErrorMessage = "Слишком длинное название заказа")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string ClientId { get; set; }

        public string VendorId { get; set; }

        [Display(Name = "Срок выполнения")]
        [DataType(DataType.Date)]
        [OrderEnd(ErrorMessage = "Некорректный срок выполнения")]
        public DateTime? End { get; set; }

        [Display(Name = "Цена")]
        [DataType(DataType.Currency)]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Некорректная цена")]
        public decimal Price { get; set; }

        public int[] TagIds { get; set; }

        public IList<TagViewModel> AllTags { get; set; }
    }

    class OrderEndAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is null || (DateTime)value > DateTime.Now.Date;
        }
    }
}
