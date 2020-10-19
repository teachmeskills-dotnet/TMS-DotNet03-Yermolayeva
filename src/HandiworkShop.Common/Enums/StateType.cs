using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.Common.Enums
{
    /// <summary>
    /// State of order.
    /// </summary>
    public enum StateType
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Completed.
        /// </summary>
        Completed = 1,

        /// <summary>
        /// In process.
        /// </summary>
        InProcess = 2,

        /// <summary>
        /// Canceled.
        /// </summary>
        Canceled = 3,

        /// <summary>
        /// Awaiting confirmation.
        /// </summary>
        AwaitingConfirm = 4
    }
}
