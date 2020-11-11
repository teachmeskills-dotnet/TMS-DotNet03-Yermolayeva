using HandiworkShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.Web.ViewModels
{
    public class TaskViewModel
    {
        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public OrderViewModel Order { get; set; }

        public int OrderId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        public StateType State { get; set; }
    }

    class TaskValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            TaskViewModel task = value as TaskViewModel;
            if (task.Start <= DateTime.Now.Date || task.End <= task.Start || task.End >= task.Order.End)
            {
                this.ErrorMessage = "Некорректная дата окончания.";
                return false;
            }
            return true;
        }
    }
}
