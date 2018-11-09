using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPrint.Models
{
    public class OrderInfoDTO
    {
        public string Id { get; set; }
        public string LayoutImageName { get; set; }
        public string Info { get; set; }
        public string Address { get; set; }
    }
}
