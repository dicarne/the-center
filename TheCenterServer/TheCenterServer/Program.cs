using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace TheCenterServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a root command with some options
            var rootCommand = new RootCommand
            {
                new Option<string>(
                    "--dbpath",
                    getDefaultValue: () => @"D://data.db",
                    description: "Where is your database."),
                new Option<string?>(
                    "--gen-api-doc",
                    getDefaultValue: () => null,
                    description: "Gen Api Doc.")
            };
            bool quit = false;
            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<string, string?>((dbpath, genapidoc) =>
            {
                WorkspaceManager.DBPath = dbpath;
                if(genapidoc != null)
                {
                    DocGen.Gen(genapidoc);
                    quit = true;
                }
            });

            // Parse the incoming args and invoke the handler
            rootCommand.Invoke(args);
            if (quit) return;

#if DEBUG
            DocGen.Gen();
#endif

            _ = new ModuleManager();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
