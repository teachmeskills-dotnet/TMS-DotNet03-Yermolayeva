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
        CanceledByClient = 3,

        /// <summary>
        /// Canceled.
        /// </summary>
        CanceledByVendor = 4,

        /// <summary>
        /// Awaiting confirm.
        /// </summary>
        AwaitingConfirm = 5,

        /// <summary>
        /// Awaiting vendor.
        /// </summary>
        AwaitingVendor = 6
    }
}
