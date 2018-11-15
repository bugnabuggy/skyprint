﻿using GeoPing.Core.Models;
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


                if (item.Image != null)
                {
                    filePath = dir + $"\\c_{valuesDict["maket"]}";

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
                    "Комментарий = " + comments.Replace("\n", "; ")
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
                Data = GetInfoDTO(valuesDict, dir),
                Success = true,
                Messages = new[] { "Edits was sended successfully" }
            };
        }

        public OperationResult<OrderImageInfoDTO> GetImage(string id)
        {
            var dir = GetOrderDirectory(id);

            var info = ParseInfoTxt(dir);
            var valuesDict = RefactorInfoData(info);

            var imageName = valuesDict["maket"];

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
        private OrderInfoDTO GetInfoDTO(Dictionary<string, string> infoData,  string directory)
        {

            var result = new OrderInfoDTO()
            {
                Name = infoData["name"],
                Picture = $"api/image/{infoData["name"]}",
                Info = infoData.FirstOrDefault(x => x.Key.Equals("dop-infa")).Value,
                Address = infoData.FirstOrDefault(x => x.Key.Equals("adress")).Value,
                TransportCompany = infoData.FirstOrDefault(x => x.Key.Equals("transport_kompany")).Value,
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
        private Dictionary<string, string> RefactorInfoData(string[] data)
        {
            var values = new Dictionary<string, string>();

            data[0] = data[0].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];
            values.Add("name", data[0]);


            foreach (var str in data)
            {
                var split = str.Split("=");
                if (split.Length < 2 )
                {
                    if(!string.IsNullOrWhiteSpace(str))
                        { values.Add(str, str);}
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
