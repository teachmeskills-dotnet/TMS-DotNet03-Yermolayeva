using HandiworkShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.DAL.Entities
{
    public class TaskBase 
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public StateType State { get; set; }
    }
}
