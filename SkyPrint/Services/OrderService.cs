using GeoPing.Core.Models;
using Microsoft.Extensions.Configuration;
using SkyPrint.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SkyPrint.Helpers;
using Microsoft.AspNetCore.Http.Features;

namespace SkyPrint.Services
{
    public class OrderService : IOrderServices
    {
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
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
                Picture = $"api/image/{infoData[0]}",
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

        public async Task<OperationResult> EditOrder(string id, OrderEditFormDTO item)
        {
            try
            {
                //var dir = GetDirectory(id);
                var dir = "D:\\test";

                var infoData = ParseInfoTxt(dir);
                infoData = RefactorInfoData(infoData);

                //
                infoData[1] = "test.jpeg";

                var filePath = dir + $"\\c_{infoData[1]}";
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await item.Image.CopyToAsync(stream);
                }

                var csaDir = GetCsaDirectory(dir);

                var content = new[]
                {
                    "Ответ = " + Responses.GetResponse(item.Status),
                    "Файл = " + filePath,
                    "Комментарий = " + item.Comments
                };

                System.IO.File.WriteAllLines(csaDir, content);
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    Messages = new[] { "ERROR: " + ex.Message }
                };
            }

            return new OperationResult()
            {
                Success = true,
                Messages = new[] { "Edits was sended successfully" }
            };
        }

        public OperationResult<OrderImageInfoDTO> GetImage(string id)
        {
            var dir = GetDirectory(id);

            var info = ParseInfoTxt(dir);
            info = RefactorInfoData(info);
            var imageName = info[1];
            var imageType = imageName.Split('.')[1];
            //var data = File.ReadAllBytes("D:\\Pictures\\1338411218-2064600-0109746_www.nevseoboi.com.ua.jpg");
            var data = Convert.FromBase64String(AnswerStab.Maket.ImageContent);

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

        private string GetCsaDirectory(string directory)
        {
            // TODO: CHANGE FILE FINDING
            var dir = System.IO.Directory.GetFiles(directory).FirstOrDefault(x => x.Contains("csa"));

            if (dir == null)
            {
                var dateNow = DateTime.UtcNow;

                dir = directory + "\\" + dateNow.ToString("yyyyMMdd_hh-mm-ss") + ".csa";
            }

            return dir;
        }

        private string[] ParseCsa(string directory)
        {
            //var dir = GetCsaDirectory(directory);
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
