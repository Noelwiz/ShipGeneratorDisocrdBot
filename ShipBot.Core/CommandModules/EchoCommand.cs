using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace ShipBot.Core.CommandModules
{
	//module w/o a prefix. 
    public class EchoCommand : ModuleBase<SocketCommandContext>
    {
		// ~say hello world -> hello world
		[Command("say")]
		[Summary("Echoes a message.")]
		public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
        {
			return ReplyAsync(echo);
		}


		// ReplyAsync is a method on ModuleBase 
	}
}
