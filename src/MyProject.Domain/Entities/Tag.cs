using MyProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.DAL.Entities
{
    public class Tag : IHasDbIdentity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Navigation to user tags.
        /// </summary>
        public ICollection<UserTags> UserTags { get; set; }
    }
}
