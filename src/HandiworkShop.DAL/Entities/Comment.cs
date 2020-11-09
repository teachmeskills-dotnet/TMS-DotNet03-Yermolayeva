using HandiworkShop.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.DAL.Entities
{
    public class Comment : IHasDbIdentity
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; }

        public DateTime Created { get; set; }

        public string AuthorId { get; set; }

        /// <summary>
        /// Navigation to author.
        /// </summary>
        public ApplicationUser Author { get; set; }

        public string ProfileId { get; set; }

        /// <summary>
        /// Navigation to profile.
        /// </summary>
        public ApplicationUser Profile { get; set; }
    }
}
