using System;

namespace HandiworkShop.BLL.Models
{
    /// <summary>
    /// Comment data transfer object.
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// Order identifier.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Author identifier.
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Rating.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Created.
        /// </summary>
        public DateTime Created { get; set; }
    }
}