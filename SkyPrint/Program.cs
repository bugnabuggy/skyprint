using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SkyPrint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args)
                .UseUrls("http://0.0.0.0:80/") // ← remove this for development
                .Build();

            host.RunAsService();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;  // ← remove this for development
            var pathToContentRoot = Path.GetDirectoryName(pathToExe); // ← remove this for development

            return WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(pathToContentRoot)  // ← remove this for development
                .UseStartup<Startup>();
        }
    }
}
