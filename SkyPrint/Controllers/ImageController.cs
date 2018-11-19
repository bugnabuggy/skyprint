using Microsoft.AspNetCore.Mvc;
using SkyPrint.Helpers;
using SkyPrint.Services;
using System;
using System.Collections.Generic;
using System.IO;
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

        // GET api/image/{id}
        // Returns model image of order
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetOrderImage(string id)
        {
            id = _idHelper.CutIdBeforeFirstLetter(id);

            if (!_orderSrv.IsOrderExistById(id))
            {
                return NotFound("Order not found");
            }

            var result = _orderSrv.GetModel(id);

            if (result.Success)
            {
                return File(result.Data.FileContent, result.Data.FileType, result.Data.FileName);
            }

            return NotFound("Model wasn`t found");
        }
    }
}
