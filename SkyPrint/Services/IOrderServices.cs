using GeoPing.Core.Models;
using SkyPrint.Models;
using System.Threading.Tasks;

namespace SkyPrint.Services
{
    public interface IOrderServices
    {
        OperationResult<OrderInfoDTO> GetInfo(string id);
        OperationResult<OrderModelInfoDTO> GetModel(string id);
        bool IsOrderExistById(string id);
        Task<OperationResult> EditOrder(string id, OrderEditFormDTO item);
    }
}
