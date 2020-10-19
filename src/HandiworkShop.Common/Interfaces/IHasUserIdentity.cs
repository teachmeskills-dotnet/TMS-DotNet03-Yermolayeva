using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.Common.Interfaces
{
    public interface IHasUserIdentity
    {
        /// <summary>
        ///User identifier.
        /// </summary>
        string UserId { get; set; }
    }
}
