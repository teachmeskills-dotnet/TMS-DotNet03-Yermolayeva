using System;
using System.ComponentModel.DataAnnotations;

namespace HandiworkShop.Web.ViewModels
{
    /// <summary>
    /// Comment view model.
    /// </summary>
    public class CommentViewModel
    {
        /// <summary>
        /// Order identifier.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        [Display(Name = "Комментарий")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        /// <summary>
        /// Rating.
        /// </summary>
        [Display(Name = "Оценка")]
        [Required(ErrorMessage = "Укажите оценку")]
        [Range(1, 5, ErrorMessage = "Укажите оценку")]
        public int Rating { get; set; }

        /// <summary>
        /// Created.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        /// <summary>
        /// Author name.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Author username.
        /// </summary>
        public string AuthorUserName { get; set; }

        /// <summary>
        /// Author avatar.
        /// </summary>
        public byte[] AuthorAvatar { get; set; }
    }
}