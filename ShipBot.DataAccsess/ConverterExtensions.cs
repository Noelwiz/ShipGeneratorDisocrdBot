using System;
using System.Collections.Generic;
using System.Text;

namespace ShipBot.DataAccsess
{
    static class ConverterExtensions
    {
        public static Domain.Character Convert(this DataAccsess.Character character)
        {
            ulong? user = null;
            if (character.Owner != null && ! string.IsNullOrWhiteSpace(character.Owner))
            {
                user = ulong.Parse(character.Owner);
            }

            Domain.Character converted = new Domain.Character(character.Name, user, character.Race, character.Description);

            return converted;
        }
    }
}
