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
        DiscordSocketClient _Client;


        public GenerateShipCommand(IShipRepository shiprepo, DiscordSocketClient client)
        {
            _Repo = shiprepo;
            _Client = client;

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
                reply.Append(_Client.GetUser( (ulong) newship.character1.DiscordUser)?.Mention);
            }

            reply.Append(" x ");

            reply.Append(newship.character2.Name);
            if (newship.character2.DiscordUser != null)
            {
                reply.Append(_Client.GetUser( (ulong) newship.character2.DiscordUser)?.Mention);
            }


            return ReplyAsync(reply.ToString());
        }
    }
}
