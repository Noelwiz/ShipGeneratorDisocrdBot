using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ShipBot.Core
{
    //https://docs.stillu.cc/guides/getting_started/first-bot.html
    /// <summary>
    /// words
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            // https://www.youtube.com/watch?v=GAOCe-2nXqc
            var host = Host.CreateDefaultBuilder(args);

            host.ConfigureServices(x =>
               {
                    //should be an interface but I don't care enough in this instance
                    x.AddTransient<ShipBot>();
               });
            //hmmm, might need to pull in bot commands to configure.

            var builthost = host.Build();

            var botsrvc = ActivatorUtilities.CreateInstance<ShipBot>(builthost.Services);

            botsrvc.RunBot().GetAwaiter().GetResult();
        }

       
    }
}
