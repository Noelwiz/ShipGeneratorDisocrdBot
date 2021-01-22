﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShipBot.Core
{
    class Character
    {
        public Character(string name, int discordUser, string race = null, string description = null)
        {
            Name = name;
            DiscordUser = discordUser;
            Race = race;
            Description = description;
        }

        string Name { get; set; }

        int DiscordUser { get; set;  }

        string Race { get; set; }

        string Description { get; set; }
    }
}