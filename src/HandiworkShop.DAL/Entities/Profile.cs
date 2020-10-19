using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;
using HandiworkShop.Common;
using HandiworkShop.Common.Interfaces;

namespace HandiworkShop.DAL.Entities
{
    /// <summary>
    /// Profile.
    /// </summary>
    public class Profile : IHasDbIdentity, IHasUserIdentity
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Navigation to application user.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <inheritdoc/>
        public string UserId { get; set; }

        public string Name { get; set; }

        public bool IsVendor { get; set; }

        public string Info { get; set; }

        public DateTime Created { get; set; }

    }
}
