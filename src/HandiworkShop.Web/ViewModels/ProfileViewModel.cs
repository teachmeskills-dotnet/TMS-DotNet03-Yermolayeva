using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.Web.ViewModels
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public bool IsVendor { get; set; }
    }
}
