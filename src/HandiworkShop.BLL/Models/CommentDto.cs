using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.BLL.Models
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; }

        public DateTime Created { get; set; }

        public string AuthorId { get; set; }

        public string ProfileId { get; set; }
    }
}
