using System;
using System.Collections.Generic;
using System.Text;

namespace ShipBot.Domain
{
    public class Ship
    {
        Character character1 { get; }

        Character character2 { get; }

        public Ship(Character char1, Character char2)
        {
            character1 = char1;
            character2 = char2;
        }
    }
}
