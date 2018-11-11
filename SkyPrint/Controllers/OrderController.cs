using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyPrint.Helpers;
using SkyPrint.Services;

namespace SkyPrint.Controllers
{
    [Route("api/order")]
    public class OrderController : Controller
    {
        private IOrderServices _orderSrv;
        private IIdHelper _idHelper;

        public OrderController(IOrderServices orderSrv,
                               IIdHelper idHelper)
        {
            _orderSrv = orderSrv;
            _idHelper = idHelper;
        }

        [HttpGet]
        [Route("info")]
        public IActionResult GetInfo(string id)
        {
            id = _idHelper.CutIdBeforeFirstLetter(id);

            var result = _orderSrv.GetInfo(id);

            if (result.Success)
            {
                return Ok(result); 
            }

            return BadRequest(result);
        }

        [HttpGet]
        [Route("image")]
        public IActionResult GetOrderImage(string id)
        {
            var result = _orderSrv.GetImage(id);

            if (result.Success)
            {
                return File(result.Data.Image, result.Data.FileType, result.Data.FileName);
            }

            return BadRequest(result);
        }
    }
}