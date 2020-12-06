﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.DAL.Entities
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
