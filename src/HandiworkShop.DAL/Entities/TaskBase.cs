using HandiworkShop.Common.Enums;
using HandiworkShop.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.DAL.Entities
{
    public class TaskBase : IHasDbIdentity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public StateType State { get; set; }
    }
}
