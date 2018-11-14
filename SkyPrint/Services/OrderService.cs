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
        private IIdHelper _idHelper;
        private string _fileHost;

        public OrderService(IConfiguration cfg,
                            IIdHelper idHelper)
        {
            _cfg = cfg;
            _idHelper = idHelper;
            _fileHost = _cfg.GetValue<string>("FileHost");
        }

        public OperationResult<OrderInfoDTO> GetInfo(string id)
        {
            var dir = GetOrderDirectory(id);

            var infoData = ParseInfoTxt(dir);
            infoData = RefactorInfoData(infoData);

            var result = GetInfoDTO(infoData, dir);

            return new OperationResult<OrderInfoDTO>()
            {
                Success = true,
                Messages = new[] { "Order was found" },
                Data = result
            };
        }

        public async Task<OperationResult> EditOrder(string id, OrderEditFormDTO item)
        {
            var dir = GetOrderDirectory(id);

            var infoData = ParseInfoTxt(dir);
            infoData = RefactorInfoData(infoData);

            try
            {
                string filePath = "";

                if (item.Image != null)
                {
                    filePath = dir + $"\\c_{infoData[1]}";

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.Image.CopyToAsync(stream);
                    }
                }

                var scaDir = GetScaDirectory(dir);

                if (scaDir == null)
                {
                    return new OperationResult()
                    {
                        Messages = new[] { "There is now file to attach edits" }
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

            return new OperationResult()
            {
                Data = GetInfoDTO(infoData, dir),
                Success = true,
                Messages = new[] { "Edits was sended successfully" }
            };
        }

        public OperationResult<OrderImageInfoDTO> GetImage(string id)
        {
            var dir = GetOrderDirectory(id);

            var info = ParseInfoTxt(dir);
            info = RefactorInfoData(info);

            var imageName = info[1];

            if (IsImageExistByInfo(imageName, dir))
            {
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

            return new OperationResult<OrderImageInfoDTO>();
        }

        // Checks if there is an order catalog and order info in host catalog by recieved id
        public bool IsOrderExistById(string id)
        {
            var dir = GetOrderDirectory(id);

            if (dir != null)
            {
                var dirFiles = System.IO.Directory.GetFiles(dir);

                if (dirFiles.Any(x => x == $"{dir}\\info.txt"))
                {
                    return true;
                }
            }

            return false;
        }

        // Checks if there is an image in pointed catalog with recieved name
        public bool IsImageExistByInfo(string name, string dir)
        {
            var files = System.IO.Directory.GetFiles(dir);

            if (files.Any(x => x == $"{dir}\\{name}"))
            {
                return true;
            }

            return false;
        }

        // Returns OrderInfoDTO based by content of info.txt and *.sca 
        private OrderInfoDTO GetInfoDTO(string[] infoData,  string directory)
        {
            var result = new OrderInfoDTO()
            {
                Name = infoData[0],
                Picture = $"api/image/{infoData[0]}",
                Info = infoData[2],
                Address = infoData[3],
                HasClientAnswer = true
            };

            var scaDir = GetScaDirectory(directory);
            if (scaDir != null)
            {
                var scaData = ParseSca(directory);
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

            return result;
        }

        // Parses a info.txt founded in order directory to string array
        private string[] ParseInfoTxt(string directory)
        {
            var data = File.ReadAllLines($"{directory}" + "\\info.txt", Encoding.UTF8);

            return data;
        }

        // Cleans infoData from waste data
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

        // Returns full path to *.sca file (or null if it doesn`t exist)
        private string GetScaDirectory(string directory)
        {
            // TODO: CHANGE FILE FINDING
            var dir = System.IO.Directory.GetFiles(directory).FirstOrDefault(x => x.EndsWith("sca"));

            return dir;
        }

        // Parses a *.sca founded in order directory to string array
        private string[] ParseSca(string directory)
        {
            var dir = GetScaDirectory(directory);
            var data = File.ReadAllLines($"{dir}", Encoding.UTF8);

            return data;
        }

        // Cleans scaData from waste data
        private string[] RefactorScaData(string[] data)
        {
            for (int i = 0; i < 3; i++)
            {
                var temp = data[i].Split(new[] { '=' });

                data[i] = new String(temp[1].Skip(1).ToArray());
            }

            return data;
        }

        // Returns an order directory in the host directory by recieved id
        private string GetOrderDirectory(string id)
        {
            var dirs = System.IO.Directory.GetDirectories(_fileHost);

            var dir = dirs.FirstOrDefault(x => id.Equals
            (
                _idHelper.CutIdBeforeFirstLetter(new String(x.Skip(_fileHost.Length + 1).ToArray()))
            ));

            return dir;
        }
    }
}
