using System;
using System.Collections.Generic;
using System.Text;

using ShipBot.Domain;

namespace ShipBot.DataAccsess
{
    public class ShipRepository : IShipRepository
    {
        Domain.Character IShipRepository.GetRandomCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
