using System;

namespace HandiworkShop.DAL.Entities
{
    /// <summary>
    /// Entity Profile.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Navigation to application user.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Avatar.
        /// </summary>
        public byte[] Avatar { get; set; }

        /// <summary>
        /// Is vendor.
        /// </summary>
        public bool IsVendor { get; set; }

        /// <summary>
        /// Info.
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Created.
        /// </summary>
        public DateTime Created { get; set; }
    }
}