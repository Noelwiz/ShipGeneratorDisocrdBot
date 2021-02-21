using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;

namespace ShipBot.Core.CommandModules
{
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
		// ~say hello world -> hello world
		[Command("Help")]
		[Summary("Gives information about commands.")]
		public Task Help([Remainder][Summary("The text to echo")] string echo)
        {
            return ReplyAsync("Not yet implemented. Sorry!");
        }


		// ReplyAsync is a method on ModuleBase 
	}
}
