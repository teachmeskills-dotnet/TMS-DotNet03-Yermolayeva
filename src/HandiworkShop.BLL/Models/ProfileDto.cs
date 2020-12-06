using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.BLL.Models
{
    /// <summary>
    /// Data transfer object (Profile).
    /// </summary>
    public class ProfileDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public byte[] Avatar { get; set; }

        public bool IsVendor { get; set; }

        public string Info { get; set; }

        public DateTime Created { get; set; }

        public IList<int> TagIds { get; set; }
    }
}
