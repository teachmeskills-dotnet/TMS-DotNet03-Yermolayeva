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

        /// <summary>
        ///Navigation to orders. 
        /// </summary>
        public ICollection<Order> VendorOrders { get; set; }

        /// <summary>
        ///Navigation to user tags. 
        /// </summary>
        public ICollection<UserTags> UserTags { get; set; }

        /// <summary>
        ///Navigation to comments. 
        /// </summary>
        public ICollection<Comment> AuthorComments { get; set; }

        /// <summary>
        ///Navigation to comments. 
        /// </summary>
        public ICollection<Comment> ProfileComments { get; set; }
    }
}
