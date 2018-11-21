using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SkyPrint.Helpers;
using SkyPrint.Models;

namespace SkyPrint.Services
{
    public class BannerService : IBannerService
    {
        private BannerCounter _counter;
        private IConfiguration _cfg;
        private string _fileHost;
        private string _leftBannersCatalogName;
        private string _centerBannersCatalogName;

        public BannerService(IConfiguration cfg)
        {
            _counter = BannerCounter.GetCounter();
            _cfg = cfg;
            _fileHost = _cfg.GetValue<string>("FileHost");
            _leftBannersCatalogName = _cfg.GetValue<string>("CatalogWithBanners:Left");
            _centerBannersCatalogName = _cfg.GetValue<string>("CatalogWithBanners:Center");
        }

        public FileDTO GetLeftBanner()
        {
            var dir = GetBannersDirectory(_leftBannersCatalogName);

            if (dir != null)
            {
                var banners = System.IO.Directory.GetFiles(dir);

                if (banners.Length > 0)
                {
                    var banner = banners[_counter.GetLeftCounter() % (uint) banners.Length];

                    var bannerExt = GetBannerFileExtension(banner);

                    return  new FileDTO()
                    {
                        FileName = $"banner_left.{bannerExt}",
                        FileType = $"image/{bannerExt}",
                        FileContent = File.ReadAllBytes(banner)
                    };
                }
            }

            return null;
        }

        public FileDTO GetCenterBanner()
        {
            var dir = GetBannersDirectory(_centerBannersCatalogName);

            if (dir != null)
            {
                var banners = System.IO.Directory.GetFiles(dir);

                if (banners.Length > 0)
                {
                    var banner = banners[_counter.GetCenterCounter() % (uint)banners.Length];

                    var bannerExt = GetBannerFileExtension(banner);

                    return new FileDTO()
                    {
                        FileName = $"banner_center.{bannerExt}",
                        FileType = $"image/{bannerExt}",
                        FileContent = File.ReadAllBytes(banner)
                    };
                }
            }

            return null;
        }

        // Returns directory to folder with banners
        private string GetBannersDirectory(string catalogName)
        {
            var dir = new System.IO.DirectoryInfo($"{_fileHost}\\{catalogName}");

            if (dir.Exists)
            {
                return dir.FullName;
            }

            return null;
        }

        // Returns model file extension
        private string GetBannerFileExtension(string fullFileName)
        {
            var ext = new System.IO.FileInfo(fullFileName).Extension.TrimStart('.');

            return ext;
        }
    }
}
