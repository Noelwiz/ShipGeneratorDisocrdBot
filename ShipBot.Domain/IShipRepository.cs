using System;
using System.Collections.Generic;
using System.Text;

namespace ShipBot.Domain
{
    public interface IShipRepository
    {
        public Character GetRandomCharacter();

        public Character GetCharacter(string name);

        public Ship GetRandomShip();

        public bool AddCharacter(string name, ulong owner);

        public bool RemoveCharacter(int index);

        public bool RemoveCharacter(string namne);

    }
}
