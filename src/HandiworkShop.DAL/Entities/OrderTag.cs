using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.DAL.Entities
{
    public class OrderTag
    {
        public int Id { get; set; }

        public int TagId { get; set; }

        /// <summary>
        /// Navigation to tag.
        /// </summary>
        public Tag Tag { get; set; }

        public int OrderId { get; set; }

        /// <summary>
        /// Navigation to order.
        /// </summary>
        public Order Order { get; set; }
    }
}
