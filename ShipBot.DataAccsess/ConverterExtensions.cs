using System;
using System.Collections.Generic;
using System.Text;

namespace ShipBot.DataAccsess
{
    static class ConverterExtensions
    {
        public static Domain.Character Convert(this DataAccsess.Character character)
        {
            Domain.Character converted = new Domain.Character(character.Name, ulong.Parse(character.Owner), character.Race, character.Description);

            return converted;
        }
    }
}
