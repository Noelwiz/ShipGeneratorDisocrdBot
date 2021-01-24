using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ShipBot.Core
{
    //https://docs.stillu.cc/guides/getting_started/first-bot.html
    /// <summary>
    /// words
    /// </summary>
    class Program
    {

        private DiscordSocketClient _client;
        private CommandService _commands;

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                // How much logging do you want to see?
                LogLevel = LogSeverity.Info,


                // If your platform doesn't have native WebSockets,
                // add Discord.Net.Providers.WS4Net from NuGet,
                // add the `using` at the top, and uncomment this line:
                //WebSocketProvider = WS4NetProvider.Instance
            });


            _commands = new CommandService(new CommandServiceConfig
            {
                // Again, log level:
                LogLevel = LogSeverity.Info,

                // There's a few more properties you can set,
                // for example, case-insensitive commands.
                CaseSensitiveCommands = false,
            });

            _client.Log += Log;
            _commands.Log += Log;

            //initialize commands
            var CommandHandeler = new BotCommands(_client, _commands);

            Task commandsReady = CommandHandeler.InstallCommandsAsync();

            var token = File.ReadAllText("./../../../bottoken.txt");

            await _client.LoginAsync(TokenType.Bot, token);

            await commandsReady;

            await _client.StartAsync();




            // Block this task until the program is closed.
            await Task.Delay(System.Threading.Timeout.Infinite);

            await _client.LogoutAsync();
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
