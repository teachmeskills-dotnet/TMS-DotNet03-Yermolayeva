using HandiworkShop.Common.Enums;
using System;
using System.Collections.Generic;

namespace HandiworkShop.BLL.Models
{
    /// <summary>
    /// Order data transfer object.
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Start.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// End.
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// State.
        /// </summary>
        public StateType State { get; set; }

        /// <summary>
        /// Price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Vendor identifier.
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Tag identifiers.
        /// </summary>
        public IList<int> TagIds { get; set; }

        /// <summary>
        /// Comment.
        /// </summary>
        public CommentDto Comment { get; set; }
    }
}