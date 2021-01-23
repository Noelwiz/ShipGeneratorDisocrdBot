using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace ShipBot.Core.CommandModules
{
    public class ExitCommand : ModuleBase<SocketCommandContext>
    {

        [Command("shutdown")]
        [Summary("Shuts the bot down.")]
        public Task ExitAsync()
        {
            ReplyAsync("Shutting down.");

            //todo: close the connection, shut down bot.
            //https://docs.stillu.cc/guides/concepts/connections.html
            
            return Task.CompletedTask;
        }
    }
}
