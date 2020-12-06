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

        public string Name { get; set; }
        public string UserName { get; set; }

        public string Info { get; set; }

        public DateTime Created { get; set; }

        public byte[] Avatar { get; set; }

        public double? Rating { get; set; }

        public int OrdersCompleted { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
        public ICollection<TagViewModel> Tags { get; set; }
    }
}
