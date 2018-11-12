using GeoPing.Core.Models;
using SkyPrint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPrint.Services
{
    public interface IOrderServices
    {
        OperationResult<OrderInfoDTO> GetInfo(string id);
        OperationResult<OrderImageInfoDTO> GetImage(string id);
        bool IsOrderExistById(string id);
    }
}
