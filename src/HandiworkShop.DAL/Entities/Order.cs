using System;
using System.Collections.Generic;

namespace HandiworkShop.DAL.Entities
{
    /// <summary>
    /// Entity Order.
    /// </summary>
    public class Order : TaskBase
    {
        /// <summary>
        /// Price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Navigation to client.
        /// </summary>
        public ApplicationUser Client { get; set; }

        /// <summary>
        /// Vendor identifier.
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Navigation to vendor.
        /// </summary>
        public ApplicationUser Vendor { get; set; }

        /// <summary>
        /// Comment text.
        /// </summary>
        public string CommentText { get; set; }

        /// <summary>
        /// Comment rating.
        /// </summary>
        public int? CommentRating { get; set; }

        /// <summary>
        /// Comment created.
        /// </summary>
        public DateTime? CommentCreated { get; set; }

        /// <summary>
        /// Navigation to tasks.
        /// </summary>
        public ICollection<Task> Tasks { get; set; }

        /// <summary>
        /// Navigation to order tags.
        /// </summary>
        public ICollection<OrderTag> OrderTags { get; set; }
    }
}