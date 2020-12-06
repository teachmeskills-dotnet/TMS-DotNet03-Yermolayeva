using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.Web.ViewModels
{
    public class CommentViewModel
    {
        public int OrderId { get; set; }

        [Display(Name ="Комментарий")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Display(Name = "Оценка")]
        [Required(ErrorMessage = "Укажите оценку")]
        [Range(1, 5)]
        public int Rating { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUserName { get; set; }

        public byte[] AuthorAvatar { get; set; }
    }
}
