using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.Common.Interfaces
{
    public interface IHasDbIdentity
    {
        /// <summary>
        ///Identifier.
        /// </summary>
        int Id { get; set; }
    }
}
