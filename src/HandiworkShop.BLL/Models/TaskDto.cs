using HandiworkShop.Common.Enums;
using System;

namespace HandiworkShop.BLL.Models
{
    /// <summary>
    /// Task data transfer object.
    /// </summary>
    public class TaskDto
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Order identifier.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

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
    }
}