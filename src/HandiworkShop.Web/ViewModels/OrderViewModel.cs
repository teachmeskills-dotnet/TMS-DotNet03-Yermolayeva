using HandiworkShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.Web.ViewModels
{
    [OrderValidation]
    public class OrderViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string ClientId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]
        public DateTime? End { get; set; }

        public StateType State { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }

    class OrderValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            OrderViewModel order = value as OrderViewModel;
            if (!(order.End is null) && order.End <= DateTime.Now.Date)
            {
                this.ErrorMessage = "Некорректная дата окончания.";
                return false;
            }
            return true;
        }
    }
}
