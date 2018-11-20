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
        private static ulong CenterBannerCounter { get; set; }

        private BannerCounter()
        {
            LeftBannerCounter = 0;
            CenterBannerCounter = 0;
        }

        public static BannerCounter GetCounter()
        {
            return counter;
        }

        public ulong GetLeftCounter()
        {
            return LeftBannerCounter++;
        }

        public ulong GetCenterCounter()
        {
            return CenterBannerCounter++;
        }
    }
}
