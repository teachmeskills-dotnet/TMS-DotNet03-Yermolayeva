using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.BLL.Models
{
    public class ProfileDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public bool IsVendor { get; set; }

        public string Info { get; set; }

        public DateTime Created { get; set; }
    }
}
