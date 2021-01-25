using System;
using System.Collections.Generic;
using System.Text;

namespace ShipBot.Domain
{
    public interface IShipRepository
    {
        public Character GetRandomCharacter();

        public Ship GetRandomShip();
    }
}
