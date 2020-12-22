using HandiworkShop.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HandiworkShop.Web.ViewModels
{
    /// <summary>
    /// Order edit view model.
    /// </summary>
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
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Некорректная цена")]
        public decimal Price { get; set; }

        public int[] TagIds { get; set; }

        public ICollection<TagViewModel> AllTags { get; set; }
    }

    internal class OrderEndAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is null || (DateTime)value >= DateTime.Now.Date;
        }
    }
}