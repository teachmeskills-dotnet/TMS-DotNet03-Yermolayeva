using HandiworkShop.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.DAL.Entities
{
    public class Order : TaskBase
    {
        public decimal Price { get; set; }

        public string ClientId { get; set; }

        /// <summary>
        /// Navigation to client.
        /// </summary>
        public ApplicationUser Client { get; set; }
        public string VendorId { get; set; }

        /// <summary>
        /// Navigation to vendor.
        /// </summary>
        public ApplicationUser Vendor { get; set; }

        public ICollection<Task> Tasks { get; set; }

        public ICollection<OrderTag> OrderTags { get; set; }

    }
}