using System;
using System.Collections.Generic;
using System.Text;

namespace ShipBot.Domain
{
    public class Character
    {
        public Character(string name, ulong? discordUser, string race = null, string description = null)
        {
            Name = name;
            DiscordUser = discordUser;
            Race = race;
            Description = description;
        }

        public string Name { get; set; }

        public ulong? DiscordUser { get; set; } = null;

        public string Race { get; set; }

        public string Description { get; set; }
    }
}
