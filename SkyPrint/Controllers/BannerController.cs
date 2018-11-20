using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyPrint.Services;

namespace SkyPrint.Controllers
{
    [Route("api/banner")]
    public class BannerController : Controller
    {
        private IBannerService _bannerSrv;

        public BannerController(IBannerService bannerSrv)
        {
            _bannerSrv = bannerSrv;
        }

        // GET api/banner/left
        [HttpGet]
        [Route("left")]
        public IActionResult GetLeftBanner()
        {
            var result = _bannerSrv.GetLeftBanner();

            if (result != null)
            {
                return File(result.FileContent, result.FileType, result.FileName);
            }

            return NotFound();
        }

        // GET api/banner/center
        [HttpGet]
        [Route("center")]
        public IActionResult GetCenterBanner()
        {
            var result = _bannerSrv.GetCenterBanner();

            if (result != null)
            {
                return File(result.FileContent, result.FileType, result.FileName);
            }

            return NotFound();
        }
    }
}
