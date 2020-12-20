using System;

namespace HandiworkShop.BLL.Models
{
    /// <summary>
    /// Profile data transfer object.
    /// </summary>
    public class ProfileDto
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