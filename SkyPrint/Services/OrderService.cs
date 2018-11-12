using GeoPing.Core.Models;
using Microsoft.Extensions.Configuration;
using SkyPrint.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyPrint.Helpers;

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
            var dir = GetDirectory(id);

            var infoData = ParseInfoTxt(dir);
            infoData = RefactorInfoData(infoData);

            var csaData = ParseCsa(dir);
            csaData = RefactorCsaData(csaData);

            var result = new OrderInfoDTO()
            {
                Name = infoData[0],
                Picture = "api/image?id=" + infoData[0],
                Info = infoData[2],
                Address = infoData[3]
            };

            if (!string.IsNullOrEmpty(csaData[0]))
            {
                result.HasClientAnswer = true;
                result.Status = csaData[0];
            }

            return new OperationResult<OrderInfoDTO>()
            {
                Success = true,
                Messages = new[] { "Order was found" },
                Data = result
            };
        }

        public OperationResult<OrderImageInfoDTO> GetImage(string id)
        {
            var dir = GetDirectory(id);

            var info = ParseInfoTxt(dir);
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

        public bool IsOrderExistById(string id)
        {
            //var dirs = System.IO.Directory.GetDirectories(_fileHost);
            var dirs = new[] { AnswerStab.Directory };

            if (dirs.Any(x => x.Contains(id)))
            {
                return true;
            }
            return false;
        }

        private string[] ParseInfoTxt(string directory)
        {
            //var data = File.ReadAllLines($"{directory}" + "\\info.txt", Encoding.UTF8);
            var data = new[]
            {
                "[1887_28_Листовки_А6_99_139мм_4_4_115г_СБОРКА_2500_2 500шт]",
                "maket = \"1887_28_Листовки_А6_99_139мм_4_4_115г_СБОРКА_2500_2 500шт.jpg\"",
                "dop-infa = \"\"",
                "adress = \"г. Тюмень, Болтенко Светлана Сергеевна\""
            };

            return data;
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

        private string[] ParseCsa(string directory)
        {
            //var dir = System.IO.Directory.GetFiles(directory).FirstOrDefault(x => x.Contains("csa"));
            //var data = File.ReadAllLines($"{dir}", Encoding.UTF8);
            var data = new[]
            {
                "Ответ = Макет одобрен",
                "Файл = ",
                "Комментарий = "
            };

            return data;
        }

        private string[] RefactorCsaData(string[] data)
        {
            for (int i = 0; i < 3; i++)
            {
                var temp = data[i].Split(new[] { '=' });
                data[i] = temp[1];
            }
            return data;
        }

        private string GetDirectory(string id)
        {
            //var dirs = System.IO.Directory.GetDirectories(_fileHost);
            var dirs = new[] { "1887_28_Листовки_А6_99_139мм_4_4_115г_СБОРКА_2500_2 500шт" };

            return dirs.FirstOrDefault(x => x.Contains(id));
        }
    }
}
