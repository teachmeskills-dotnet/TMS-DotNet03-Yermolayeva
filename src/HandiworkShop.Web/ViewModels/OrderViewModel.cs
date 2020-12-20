using HandiworkShop.Common.Enums;
using System;
using System.Collections.Generic;

namespace HandiworkShop.Web.ViewModels
{
    /// <summary>
    /// Order view model.
    /// </summary>
    public class OrderViewModel
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Client username.
        /// </summary>
        public string ClientUserName { get; set; }

        /// <summary>
        /// Client name.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Client avatar.
        /// </summary>
        public byte[] ClientAvatar { get; set; }

        /// <summary>
        /// Client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Vendor username.
        /// </summary>
        public string VendorUserName { get; set; }

        /// <summary>
        /// Vendor name.
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// Vendor avatar.
        /// </summary>
        public byte[] VendorAvatar { get; set; }

        /// <summary>
        /// Vendor identifier.
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Start.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// End.
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// State.
        /// </summary>
        public StateType State { get; set; }

        /// <summary>
        /// Price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Comment.
        /// </summary>
        public CommentViewModel Comment { get; set; }

        /// <summary>
        /// Tasks.
        /// </summary>
        public IEnumerable<TaskViewModel> Tasks { get; set; }

        /// <summary>
        /// Tags.
        /// </summary>
        public IEnumerable<TagViewModel> Tags { get; set; }
    }
}