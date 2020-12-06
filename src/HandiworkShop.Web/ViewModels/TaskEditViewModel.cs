using HandiworkShop.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.Web.ViewModels
{
    [TaskEnd(ErrorMessage = "Некорректный срок окончания выполнения")]
    public class TaskEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название задания")]
        [Required(ErrorMessage = "Введите название задания")]
        [MaxLength(ConfigurationConstants.StandartLenghtForStringField, ErrorMessage = "Слишком длинное название задания")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        public int OrderId { get; set; }

        [Display(Name = "Начало выполнения")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Выберите срок начала выполнения")]
        [TaskStart(ErrorMessage = "Некорректный срок начала выполнения")]
        public DateTime Start { get; set; }

        [Display(Name = "Срок выполнения")]
        [DataType(DataType.Date)]
        public DateTime? End { get; set; }
    }

    class TaskStartAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (DateTime)value > DateTime.Now.Date;
        }
    }

    class TaskEndAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var task = (TaskEditViewModel)value;
            return task.End is null || task.End > task.Start;
        }
    }
}
