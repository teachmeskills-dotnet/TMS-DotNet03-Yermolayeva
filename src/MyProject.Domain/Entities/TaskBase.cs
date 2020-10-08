using MyProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.DAL.Entities
{
    public class TaskBase : IHasDbIdentity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int State { get; set; }
    }
}
