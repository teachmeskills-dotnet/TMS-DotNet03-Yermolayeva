using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.DAL.Entities
{
    public class Tag 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Navigation to user tags.
        /// </summary>
        public ICollection<UserTag> UserTags { get; set; }

        /// <summary>
        /// Navigation to order tags.
        /// </summary>
        public ICollection<OrderTag> OrderTags { get; set; }
    }
}
