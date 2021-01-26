using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using ShipBot.Domain;

namespace ShipBot.Core.CommandModules
{
    public class GenerateShipCommand: ModuleBase<SocketCommandContext>
    {
        IShipRepository _Repo;

        public GenerateShipCommand(IShipRepository shiprepo)
        {
            _Repo = shiprepo;
        }

        [Command("ship")]
        [Summary("Creates a new ship.")]
        public Task ExitAsync()
        {
            Ship newship = _Repo.GetRandomShip();
            StringBuilder reply = new StringBuilder();

            //char 1
            reply.Append(newship.character1.Name);
            if(newship.character1.DiscordUser != null)
            {
                reply.Append(Context.Client.GetUser( (ulong) newship.character1.DiscordUser)?.Mention);
            }

            reply.Append(" x ");

            reply.Append(newship.character2.Name);
            if (newship.character2.DiscordUser != null)
            {
                reply.Append(Context.Client.GetUser( (ulong) newship.character2.DiscordUser)?.Mention);
            }


            return ReplyAsync(reply.ToString());
        }
    }
}
