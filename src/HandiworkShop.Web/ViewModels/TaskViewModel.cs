using HandiworkShop.Common.Constants;
using HandiworkShop.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HandiworkShop.Web.ViewModels
{
    /// <summary>
    /// Task edit view model.
    /// </summary>
    [TaskEnd(ErrorMessage = "Некорректный срок окончания выполнения")]
    public class TaskViewModel
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        [Display(Name = "Название задания")]
        [Required(ErrorMessage = "Введите название задания")]
        [MaxLength(ConfigurationConstants.StandartLenghtForStringField, ErrorMessage = "Слишком длинное название задания")]
        public string Title { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [Display(Name = "Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Order identifier.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Start.
        /// </summary>
        [Display(Name = "Начало выполнения")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Выберите срок начала выполнения")]
        [TaskStart(ErrorMessage = "Некорректный срок начала выполнения")]
        public DateTime Start { get; set; }

        /// <summary>
        /// End.
        /// </summary>
        [Display(Name = "Окончание выполнения")]
        [DataType(DataType.Date)]
        public DateTime? End { get; set; }

        /// <summary>
        /// State.
        /// </summary>
        public StateType State { get; set; }
    }

    internal class TaskStartAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (DateTime)value >= DateTime.Now.Date;
        }
    }

    internal class TaskEndAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var task = (TaskViewModel)value;
            return task.End is null || task.End >= task.Start;
        }
    }
}