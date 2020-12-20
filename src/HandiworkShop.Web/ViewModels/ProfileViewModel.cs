using System;
using System.Collections.Generic;

namespace HandiworkShop.Web.ViewModels
{
    /// <summary>
    /// Profile view model.
    /// </summary>
    public class ProfileViewModel
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
        /// Is vendor.
        /// </summary>
        public bool IsVendor { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Info.
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Avatar.
        /// </summary>
        public byte[] Avatar { get; set; }

        /// <summary>
        /// Rating.
        /// </summary>
        public double? Rating { get; set; }

        /// <summary>
        /// Orders completed.
        /// </summary>
        public int OrdersCompleted { get; set; }

        /// <summary>
        /// Comments.
        /// </summary>
        public ICollection<CommentViewModel> Comments { get; set; }

        /// <summary>
        /// Tags.
        /// </summary>
        public ICollection<TagViewModel> Tags { get; set; }
    }
}