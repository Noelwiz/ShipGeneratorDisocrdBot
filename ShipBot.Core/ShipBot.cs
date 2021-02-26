using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Configuration;

namespace ShipBot.Core
{
    class ShipBot
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IConfiguration _config;

        public ShipBot(IConfiguration Configuration)
        {
            _config = Configuration;
        }


        public async Task RunBot()
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

            /*ConfigurationManager.AppSettings.Keys;

            var thing = ConfigurationManager.ConnectionStrings;
            */
            string BotTokenFileLocation = _config.GetSection("BotToken").GetSection("Location").Value;

            //var token = File.ReadAllText("./../../../bottoken.txt");
            var token = File.ReadAllText(BotTokenFileLocation);

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
