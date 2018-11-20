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


            return Ok();
        }

        // GET api/banner/end
        [HttpGet]
        [Route("end")]
        public IActionResult GetEndBanner()
        {

            return Ok();
        }
    }
}
