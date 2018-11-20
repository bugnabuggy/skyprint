using Microsoft.AspNetCore.Mvc;
using SkyPrint.Helpers;
using SkyPrint.Models;
using SkyPrint.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // GET api/order/{id}
        // Returns info about order if id is right
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

        // POST api/order/{id}
        // Adds info about client wishes
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> AddOrderEditsAsync(string id, [FromForm]OrderEditFormDTO item)
        {
            id = _idHelper.CutIdBeforeFirstLetter(id);

            if (!_orderSrv.IsOrderExistById(id))
            {
                return NotFound("Order not found");
            }

            var result = await _orderSrv.EditOrder(id, item);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}