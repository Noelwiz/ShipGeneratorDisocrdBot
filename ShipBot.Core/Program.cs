using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

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
            Console.WriteLine("Hello World!");
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            
            var token = File.ReadAllText("./../../../bottoken.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }


        private DiscordSocketClient _client;


    }
}
