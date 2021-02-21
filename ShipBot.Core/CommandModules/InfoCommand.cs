using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using ShipBot.Domain;

namespace ShipBot.Core.CommandModules
{
    public class InfoCommand : ModuleBase<SocketCommandContext>
    {

        IShipRepository _Repo;


        public InfoCommand(IShipRepository shiprepo)
        {
            _Repo = shiprepo;
        }

        [Command("info")]
        [Summary("Sends info about a character.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string CharName)
        {
            CharName = CharName.Trim();

            var character = _Repo.GetCharacter(CharName);

            if (character is not null)
            {
                return ReplyAsync($"Added the character {CharName}, and asigned ownership to you.");
            }
            else
            {
                return ReplyAsync($"Failed to add {CharName} for some reason.");
            }

        }


    }
}
