using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using ShipBot.Domain;

namespace ShipBot.Core.CommandModules
{
    public class InfoCommand : ModuleBase<SocketCommandContext>
    {

        IShipRepository _Repo;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="shiprepo"></param>
        public InfoCommand(IShipRepository shiprepo)
        {
            _Repo = shiprepo;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CharName"></param>
        /// <seealso cref="https://docs.stillu.cc/api/Discord.EmbedBuilder.html"/>
        /// <seealso cref="https://leovoel.github.io/embed-visualizer/"/>
        /// <returns></returns>
        [Command("info")]
        [Summary("Sends info about a character.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string CharName)
        {
            CharName = CharName.Trim();

            var character = _Repo.GetCharacter(CharName);

            if (character is not null)
            {
                var embed = new EmbedBuilder();

                embed.Color = Color.LightOrange;
                embed.Title = character.Name;

                var embedauthor = new EmbedAuthorBuilder();
                
                if (character.DiscordUser is not null)
                { 
                    embedauthor.Name = Context.Client.GetUser((ulong) character.DiscordUser)?.Username ?? "Unknown/Offline Creator";
                } else
                {
                    embedauthor.Name = this.Context.User.Username;
                }

                embed.Author = embedauthor;
                embed.WithCurrentTimestamp();

                if(! string.IsNullOrWhiteSpace(character.Race))
                {
                    var RaceField = new EmbedFieldBuilder().WithIsInline(false).WithName("Race").WithValue(character.Race);
                    embed.AddField(RaceField);
                }

                if(!string.IsNullOrWhiteSpace(character.Description))
                {
                    int remaining_Length = EmbedBuilder.MaxDescriptionLength - embed.Length;

                    if (remaining_Length > character.Description.Length)
                    {
                        embed.Description = character.Description;
                    } else if(remaining_Length > 1)
                    {
                        embed.Description = character.Description.Substring(0, remaining_Length - 1);
                    }
                    //else this will probably throw an error, but idk.
                }
                
                //embed.AddField(name: "name", value: null, inline: false);
                return ReplyAsync(embed: embed.Build());
            }
            else
            {
                return ReplyAsync($"Failed to find {CharName}.");
            }

        }


    }
}
