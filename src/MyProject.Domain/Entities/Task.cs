using MyProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.DAL.Entities
{
    public class Task : TaskBase
    {
        public int OrderId { get; set; }

        /// <summary>
        /// Navigation to order.
        /// </summary>
        public Order Order { get; set; }
    }
}
