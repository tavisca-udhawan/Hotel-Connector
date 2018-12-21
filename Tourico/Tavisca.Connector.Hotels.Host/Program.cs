using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Tavisca.Connector.Hotels.Host
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            var host = new WebHostBuilder()
                          .UseKestrel()
                          .UseConfiguration(config)
                          .UseContentRoot(AppContext.BaseDirectory)
                          .UseStartup<Startup>()
                          .Build();
            host.Run();
        }
    }
}
