using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPrint.Models
{
    public class OrderEditFormDTO
    {
        public string Comments { get; set; }
        public int Status { get; set; }
        public IFormFile Image { get; set; }
    }
}
