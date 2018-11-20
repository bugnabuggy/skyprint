using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkyPrint.Models;

namespace SkyPrint.Services
{
    public interface IBannerService
    {
        FileDTO GetLeftBanner();
        FileDTO GetCenterBanner();
    }
}
