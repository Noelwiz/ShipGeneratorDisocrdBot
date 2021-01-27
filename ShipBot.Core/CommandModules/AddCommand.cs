using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using ShipBot.Domain;

namespace ShipBot.Core.CommandModules
{
    public class AddCommand : ModuleBase<SocketCommandContext>
    {

        IShipRepository _Repo;


        public AddCommand(IShipRepository shiprepo)
        {
            _Repo = shiprepo;
        }

        [Command("add")]
        [Summary("Adds a character.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string charname)
        {
            var result = _Repo.AddCharacter(charname, Context.Message.Author.Id);

            if (result)
            {
                return ReplyAsync($"Added the character {charname}, and asigned ownership to you.");
            } else
            {
                return ReplyAsync($"Failed to add {charname} for some reason.");
            }
            
        }
    }
}
