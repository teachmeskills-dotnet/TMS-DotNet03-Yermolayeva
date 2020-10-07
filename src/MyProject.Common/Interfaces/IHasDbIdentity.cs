using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.Common.Interfaces
{
    public interface IHasDbIdentity
    {
        /// <summary>
        ///Identifier.
        /// </summary>
        int Id { get; set; }
    }
}
