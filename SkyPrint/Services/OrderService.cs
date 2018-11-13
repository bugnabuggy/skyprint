using GeoPing.Core.Models;
using Microsoft.Extensions.Configuration;
using SkyPrint.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SkyPrint.Helpers;
using Microsoft.AspNetCore.Http.Features;
using System.Text;

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

            var result = new OrderInfoDTO()
            {
                Name = infoData[0],
                Picture = $"api/image/{infoData[0]}",
                Info = infoData[2],
                Address = infoData[3],
                HasClientAnswer = true
            };

            var scaDir = GetScaDirectory(dir);
            if (scaDir != null)
            {
                var scaData = ParseSca(dir);
                scaData = RefactorScaData(scaData);

                if (!string.IsNullOrEmpty(scaData[0]))
                {
                    result.Status = scaData[0];
                }
                else
                {
                    result.HasClientAnswer = false;
                }
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
                var dir = GetDirectory(id);

                var infoData = ParseInfoTxt(dir);
                infoData = RefactorInfoData(infoData);

                var filePath = dir + $"\\c_{infoData[1]}";
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await item.Image.CopyToAsync(stream);
                }

                var scaDir = GetScaDirectory(dir);

                if (scaDir == null)
                {
                    return new OperationResult()
                    {
                        Messages = new []{"There is now file to attach edits"}
                    };
                }

                var content = new[]
                {
                    "Ответ = " + Responses.GetResponse(item.Status),
                    "Файл = " + filePath,
                    "Комментарий = " + item.Comments
                };

                using (TextWriter fileTW = new StreamWriter(scaDir))
                {
                    fileTW.NewLine = "\n";
                    foreach (string s in content)
                    {
                        fileTW.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    Messages = new[] { $"ERROR: {ex.Message}" }
                };
            }

            item.Image = null;
            return new OperationResult()
            {
                Data = item,
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
            var data = File.ReadAllBytes($"{dir}\\{imageName}");

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
            var dirs = System.IO.Directory.GetDirectories(_fileHost);
            
            if (dirs.Any(x => x.Contains(id)))
            {
                return true;
            }

            return false;
        }

        private string[] ParseInfoTxt(string directory)
        {
            var data = File.ReadAllLines($"{directory}" + "\\info.txt", Encoding.UTF8);

            return data;
        }

        private string[] RefactorInfoData(string[] data)
        {
            data[0] = data[0].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];

            for (int i = 1; i <= 3; i++)
            {
                var temp = data[i].Split('\"');

                data[i] = temp.Length > 0
                    ? temp[1]
                    : null;
            }
            return data;
        }

        private string GetScaDirectory(string directory)
        {
            // TODO: CHANGE FILE FINDING
            var dir = System.IO.Directory.GetFiles(directory).FirstOrDefault(x => x.Contains("sca"));

            return dir;
        }

        private string[] ParseSca(string directory)
        {
            var dir = GetScaDirectory(directory);
            var data = File.ReadAllLines($"{dir}", Encoding.UTF8);

            return data;
        }

        private string[] RefactorScaData(string[] data)
        {
            for (int i = 0; i < 3; i++)
            {
                var temp = data[i].Split(new[] { '=' });
                
                data[i] =  temp[1];
            }

            return data;
        }

        private string GetDirectory(string id)
        {
            var dirs = System.IO.Directory.GetDirectories(_fileHost);

            // TODO: REFACTOR THIS PIECE OF CRAP
            return dirs.FirstOrDefault(x => x.Contains(id));
        }
    }
}
