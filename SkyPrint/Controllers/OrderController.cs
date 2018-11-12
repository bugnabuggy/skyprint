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
        [Route("{id}")]
        public IActionResult GetInfo(string id)
        {
            id = _idHelper.CutIdBeforeFirstLetter(id);

            if (!_orderSrv.IsOrderExistById(id))
            {
                return NotFound("Order not found");
            }

            var result = _orderSrv.GetInfo(id);

            if (result.Success)
            {
                return Ok(result); 
            }

            return BadRequest(result);
        }
    }
}