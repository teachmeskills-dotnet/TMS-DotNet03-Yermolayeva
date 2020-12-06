using HandiworkShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.Web.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ClientUserName { get; set; }

        public byte[] ClientAvatar { get; set; }

        public string ClientId { get; set; }

        public string VendorUserName { get; set; }

        public byte[] VendorAvatar { get; set; }

        public string VendorId { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public StateType State { get; set; }

        public decimal Price { get; set; }

        public bool HasComment { get; set; }

        public IEnumerable<TaskViewModel> Tasks { get; set; }

        public IEnumerable<TagViewModel> Tags { get; set; }

    }
}
