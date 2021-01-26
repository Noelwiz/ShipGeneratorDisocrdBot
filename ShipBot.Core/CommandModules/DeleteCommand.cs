using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ShipBot.Domain;

namespace ShipBot.Core.CommandModules
{
    class DeleteCommand : ModuleBase<SocketCommandContext>
    {

        IShipRepository _Repo;


        public DeleteCommand(IShipRepository shiprepo)
        {
            _Repo = shiprepo;
        }


        [Command("AdminRemove")]
        [RequireUserPermission(Discord.GuildPermission.ManageMessages)]
        [Summary("Removes a character a character.")]
        public Task AdminRemove([Remainder] string CharacterName)
        {
            var result =  _Repo.RemoveCharacter(CharacterName);

            if (result)
            {
                return ReplyAsync($"Succsessfully removed {CharacterName}");
            } else
            {
                return ReplyAsync($"Failed to remove {CharacterName}, probably not found.");
            }
            
        }

        [Command("remove")]
        [Summary("Adds a character.")]
        public Task Remove(IUser user, [Remainder][Summary("The text to echo")] string name)
        {
            ulong? userid = Context.User.Id;

            name = name.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                return ReplyAsync("Invalid name.");
            }

            Character c =_Repo.GetCharacter(name);
            if(c == null)
            {
                return ReplyAsync("Failed to find that character.");
            }

            //check if the sender owns the character
            if(userid != c.DiscordUser && c.DiscordUser != null)
            {
                return ReplyAsync("You do not have premission to remove that character.");
            }

            //if so, remove
            _Repo.RemoveCharacter(name);

            return ReplyAsync($"Succsesfully removed the character, {name}.");
        }



    }
}
