using HandiworkShop.Common.Enums;
using System;

namespace HandiworkShop.DAL.Entities
{
    /// <summary>
    /// Task base.
    /// </summary>
    public class TaskBase
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