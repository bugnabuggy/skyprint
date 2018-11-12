﻿using Microsoft.AspNetCore.Mvc;
using SkyPrint.Helpers;
using SkyPrint.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPrint.Controllers
{
    [Route("api/image")]
    public class ImageController : Controller
    {
        private IOrderServices _orderSrv;
        private IIdHelper _idHelper;

        public ImageController(IOrderServices orderSrv,
            IIdHelper idHelper)
        {
            _orderSrv = orderSrv;
            _idHelper = idHelper;
        }

        [HttpGet]
        public IActionResult GetOrderImage(string id)
        {
            id = _idHelper.CutIdBeforeFirstLetter(id);

            if (!_orderSrv.IsOrderExistById(id))
            {
                return NotFound("Order not found");
            }

            var result = _orderSrv.GetImage(id);

            if (result.Success)
            {
                return File(result.Data.Image, result.Data.FileType, result.Data.FileName);
            }

            return BadRequest(result);
        }
    }
}