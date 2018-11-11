using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPrint.Models
{
    public class OrderImageInfoDTO
    {
        public byte[] Image { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
    }
}
