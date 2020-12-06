using HandiworkShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.BLL.Models
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public StateType State { get; set; }

        public decimal Price { get; set; }

        public string ClientId { get; set; }
        public string VendorId { get; set; }

        public IList<int> TagIds { get; set; }

        public CommentDto Comment { get; set; }
    }
}
