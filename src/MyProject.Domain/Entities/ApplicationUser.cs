using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.DAL.Entities
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
        ///Navigation to orders. 
        /// </summary>
        public ICollection<Order> ClientOrders { get; set; }

        public ICollection<Order> VendorOrders { get; set; }
    }
}
