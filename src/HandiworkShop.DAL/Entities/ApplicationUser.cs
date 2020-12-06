using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace HandiworkShop.DAL.Entities
{
    /// <summary>
    /// Application user.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Navigation to profile.
        /// </summary>
        public Profile Profile { get; set; }

        /// <summary>
        ///Navigation to client orders. 
        /// </summary>
        public ICollection<Order> ClientOrders { get; set; }

        /// <summary>
        ///Navigation to vendor orders. 
        /// </summary>
        public ICollection<Order> VendorOrders { get; set; }

        /// <summary>
        ///Navigation to user tags. 
        /// </summary>
        public ICollection<UserTag> UserTags { get; set; }
    }
}
