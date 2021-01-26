using System;
using System.Linq;

using ShipBot.Domain;

namespace ShipBot.DataAccsess
{
    public class ShipRepository : IShipRepository
    {
        private ShipDbContext _context;
        private Random _rng;

        public ShipRepository(ShipDbContext context)
        {
            _context = new ShipDbContext();
            _rng = new Random();
        }

        public Domain.Ship GetRandomShip()
        {
            Domain.Character char1 = GetRandomCharacter();
            Domain.Character char2;
            do
            {
                char2 = GetRandomCharacter();
            } while (char1 == char2);


            return new Domain.Ship(char1, char2);
        }


        public Domain.Character GetRandomCharacter()
        {
            int maxId =  (int) _context.Characters.Max(c => c.Id);
            Character dbchar = null;

            do
            {
                dbchar = _context.Characters.Find( (long) _rng.Next(maxId));
            } while (dbchar == null);

            return dbchar.Convert();
        }
    }
}
