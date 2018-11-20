using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPrint.Helpers
{
    public class BannerCounter
    {
        private static readonly BannerCounter counter = new BannerCounter();

        private static ulong LeftBannerCounter { get; set; }
        private static ulong EndBannerCounter { get; set; }

        private BannerCounter()
        {
            LeftBannerCounter = 0;
            EndBannerCounter = 0;
        }

        public static BannerCounter GetCounter()
        {
            return counter;
        }

        public static ulong GetLeftCounter()
        {
            return LeftBannerCounter++;
        }

        public static ulong GetEndCounter()
        {
            return EndBannerCounter++;
        }
    }
}
