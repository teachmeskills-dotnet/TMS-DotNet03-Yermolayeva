using System.Collections.Generic;

namespace HandiworkShop.DAL.Entities
{
    /// <summary>
    /// Entity tag.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
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