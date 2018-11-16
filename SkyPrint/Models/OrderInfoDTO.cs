using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPrint.Models
{
    public class OrderInfoDTO
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public string FileType { get; set; }
        public string Info { get; set; }
        public string Address { get; set; }
        public string TransportCompany { get; set; }
        public bool HasClientAnswer { get; set; }
        public string Status { get; set; }
    }
}
