using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;


namespace ShipBot.Core.CommandModules
{
    public class AddCommand : ModuleBase<SocketCommandContext>
    {

        [Command("add")]
        [Summary("Adds a character.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
        {
            return ReplyAsync(echo);
        }
    }
}
