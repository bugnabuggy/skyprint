using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPrint.Helpers
{
    public class AnswerStab
    {
        public static string Directory { get; set; }
        public static LayoutImage Maket { get; set; }
        public static InfoTxt Info { get; set; }
        public static InfoSca Answer { get; set; }
    }

    public class InfoSca
    {
        public string Answer { get; set; }
        public string FileName { get; set; }
        public string Comments { get; set; }
    }

    public class InfoTxt
    {
        public string Id { get; set; }
        public string ImageName { get; set; }
        public string AddInfo { get; set; }
        public string Address { get; set; }
    }

    public class LayoutImage
    {
        public string ImageName { get; set; }
        public string ImageType { get; set; }
        public string ImageContent { get; set; }
    }
}
