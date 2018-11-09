using GeoPing.Core.Models;
using Microsoft.Extensions.Configuration;
using SkyPrint.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyPrint.Services
{
    public class OrderService : IOrderServices
    {
        private IConfiguration _cfg;
        private string _fileHost;

        public OrderService(IConfiguration cfg)
        {
            _cfg = cfg;
            _fileHost = _cfg.GetValue<string>("FileHost");
        }

        public OperationResult<OrderInfoDTO> GetInfo(string id)
        {
            if (!IsOrderExistById(id, out var dir))
            {
                return new OperationResult<OrderInfoDTO>() { Messages = new[] { "Order not found" } };
            }

            var data = File.ReadAllLines($"{dir}" + "\\info.txt", Encoding.UTF8);
            data = RefactorInfoData(data);

            return new OperationResult<OrderInfoDTO>()
            {
                Success = true,
                Messages = new[] { "Order was found" },
                Data = new OrderInfoDTO()
                {
                    Id = data[0],
                    LayoutImageName = data[1],
                    Info = data[2],
                    Address = data[3]
                }
            };
        }

        public OperationResult<OrderImageInfoDTO> GetImage(string id)
        {
            if (!IsOrderExistById(id, out var dir))
            {
                return new OperationResult<OrderImageInfoDTO>() { Messages = new[] { "Order not found" } };
            }

            var info = File.ReadAllLines($"{dir}" + "\\info.txt", Encoding.UTF8);
            info = RefactorInfoData(info);

            var imageName = info[1];
            var imageType = imageName.Split('.')[1];
            var data = File.ReadAllBytes($"{dir}" + $"\\{imageName}");

            return new OperationResult<OrderImageInfoDTO>()
            {
                Success = true,
                Messages = new[] { "Image was found" },
                Data = new OrderImageInfoDTO()
                {
                    Image = data,
                    FileName = imageName,
                    FileType = $"image/{imageType}",
                }
            };
        }

        private string[] RefactorInfoData(string[] data)
        {
            data[0] = data[0].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];

            for (int i = 1; i <= 3; i++)
            {
                data[i] = data[i].Split('\"')[1];
            }
            return data;
        }

        private bool IsOrderExistById(string id, out string directory)
        {
            var dirs = System.IO.Directory.GetDirectories(_fileHost);

            directory = dirs.FirstOrDefault(x => x.Contains(id));

            if (directory == null)
            {
                return false;
            }
            return true;
        }
    }
}
