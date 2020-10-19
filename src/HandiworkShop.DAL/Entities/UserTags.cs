using HandiworkShop.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.DAL.Entities
{
    public class UserTags : IHasDbIdentity, IHasUserIdentity
    {
        public int Id { get; set; }

        public int TagId { get; set; }

        /// <summary>
        /// Navigation to tag.
        /// </summary>
        public Tag Tag { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// Navigation to user.
        /// </summary>
        public ApplicationUser User { get; set; }
    }
}
