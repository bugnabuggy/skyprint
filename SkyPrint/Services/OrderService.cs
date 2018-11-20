using GeoPing.Core.Models;
using Microsoft.Extensions.Configuration;
using SkyPrint.Models;
using System;
using System.Collections.Generic;
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
            var valuesDict = RefactorInfoData(infoData);

            var result = GetInfoDTO(valuesDict, dir);

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
            var valuesDict = RefactorInfoData(infoData);

            try
            {
                var filePath = "";
                var comments = item.Comments ?? "";

                var fileExtension = GetModelFileExtension(item.Image.FileName);

                var targetFilename = valuesDict["maket"]
                    .TrimEnd(GetModelFileExtension(valuesDict["maket"]).ToCharArray());

                if (item.Image != null)
                {
                    filePath = dir + $"\\c_{targetFilename}{fileExtension}";

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.Image.CopyToAsync(stream);
                    }
                }

                var dirs = System.IO.Directory.GetDirectories(_fileHost);
                var catalogWithMarks = _cfg.GetValue<string>("CatalogWithMarks");

                var dirCatalogWithMarks = dirs.FirstOrDefault(x =>
                    catalogWithMarks.Equals(new String(x.Skip(_fileHost.Length + 1).ToArray())));

                if (dirCatalogWithMarks == null)
                {
                    dirCatalogWithMarks = $"{_fileHost}\\{catalogWithMarks}";
                    System.IO.Directory.CreateDirectory(dirCatalogWithMarks);
                }

                var dateTimeStamp = DateTime.UtcNow.ToString("yyyyMMdd_HH_mm_ss");

                using (TextWriter fileTW = File.CreateText($"{dirCatalogWithMarks}\\{dateTimeStamp}.sca"))
                {
                    fileTW.NewLine = "\n";
                    fileTW.WriteLine(valuesDict["name"]);
                }

                var content = new[]
                {
                    "Ответ = " + Responses.GetResponse(item.Status),
                    "Файл = " + filePath,
                    "Комментарий = " + comments.Replace("\n", "; ")
                };

                using (TextWriter fileTW = new StreamWriter($"{dir}\\{dateTimeStamp}.sca"))
                {
                    fileTW.NewLine = "\n";
                    foreach (string s in content)
                    {
                        fileTW.WriteLine(s);
                    }
                }

                using (TextWriter fileTW = new StreamWriter($"{dir}\\ok.txt"))
                {
                    fileTW.NewLine = "\n";
                    fileTW.WriteLine();
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
                Data = GetInfoDTO(valuesDict, dir),
                Success = true,
                Messages = new[] { "Edits was sended successfully" }
            };
        }

        public OperationResult<OrderModelInfoDTO> GetModel(string id)
        {
            var dir = GetOrderDirectory(id);

            var info = ParseInfoTxt(dir);
            var valuesDict = RefactorInfoData(info);

            var modelName = valuesDict["maket"];

            if (IsModelExistByInfo(modelName, dir))
            {
                var fileExtension = GetModelFileExtension(modelName);
                var data = File.ReadAllBytes($"{dir}\\{modelName}");

                var fileType = fileExtension == "pdf"
                    ? "application"
                    : "image";

                return new OperationResult<OrderModelInfoDTO>()
                {
                    Success = true,
                    Messages = new[] { "Model was found" },
                    Data = new OrderModelInfoDTO()
                    {
                        FileContent = data,
                        FileName = modelName,
                        FileType = $"{fileType}/{fileExtension}",
                    }
                };
            }

            return new OperationResult<OrderModelInfoDTO>();
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
        public bool IsModelExistByInfo(string name, string dir)
        {
            var files = System.IO.Directory.GetFiles(dir);

            if (files.Any(x => x == $"{dir}\\{name}"))
            {
                return true;
            }

            return false;
        }

        // Returns OrderInfoDTO based by content of info.txt and *.sca 
        private OrderInfoDTO GetInfoDTO(Dictionary<string, string> infoData, string directory)
        {

            var result = new OrderInfoDTO()
            {
                Name = infoData["name"],
                Picture = $"api/image/{infoData["name"]}",
                FileType = GetModelFileExtension(infoData.FirstOrDefault(x => x.Key.Equals("maket")).Value),
                Info = infoData.FirstOrDefault(x => x.Key.Equals("dop-infa")).Value,
                Address = infoData.FirstOrDefault(x => x.Key.Equals("adress")).Value,
                TransportCompany = infoData.FirstOrDefault(x => x.Key.Equals("transport_kompany")).Value,
            };

            if (File.Exists($"{directory}\\ok.txt"))
            {
                result.HasClientAnswer = true;

                var scaDir = GetScaDirectory(directory);
            
                if (scaDir != null)
                {
                    var scaData = ParseSca(directory);
                    scaData = RefactorScaData(scaData);

                    result.Status = scaData[0];
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
        private Dictionary<string, string> RefactorInfoData(string[] data)
        {
            var values = new Dictionary<string, string>();

            data[0] = data[0].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];
            values.Add("name", data[0]);


            foreach (var str in data)
            {
                var split = str.Split("=");
                if (split.Length < 2)
                {
                    if (!string.IsNullOrWhiteSpace(str))
                    { values.Add(str, str); }
                    continue;
                }
                else
                {
                    values.Add(split[0].Trim(), split[1].Replace("\"", "").Trim());
                }

            }

            //for (int i = 1; i <= 3; i++)
            //{
            //    var temp = data[i].Split('\"');

            //    data[i] = temp.Length > 0
            //        ? temp[1]
            //        : null;
            //}
            return values;
        }

        // Returns full path to *.sca file (or null if it doesn`t exist)
        private string GetScaDirectory(string directory)
        {
            try
            {
                var dir = System.IO.Directory.GetFiles(directory)
                        .OrderByDescending(x => x)
                        .FirstOrDefault(x => x.EndsWith("sca"));

                return dir;
            }
            catch (Exception)
            {
                return null;
            }
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

                if (temp.Length > 1)
                {
                    data[i] = temp[1].Trim();
                }
                else
                {
                    data[i] = null;
                }
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

        // Returns model file extension
        private string GetModelFileExtension(string fileName)
        {
            var ext = fileName.Split('.', StringSplitOptions.RemoveEmptyEntries).Last();

            return ext;
        }
    }
}
