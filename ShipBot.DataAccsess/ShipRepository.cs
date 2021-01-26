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



        public bool AddCharacter(string name, ulong owner)
        {
            Character newc = _context.Characters.FirstOrDefault(c => c.Name == name);

            if(newc != null)
            {
                return false;
            }

            newc = new Character();
            newc.Name = name;

            string ownerID = owner.ToString();

            Owner DBOwner = _context.Owners.Find(ownerID);
            if(DBOwner == null)
            {
                DBOwner = new Owner();
                DBOwner.DiscordUser = ownerID;
                _context.Add(DBOwner);
            }

            newc.Owner = owner.ToString();
            newc.OwnerNavigation = DBOwner;
            _context.SaveChanges();

            return true;
        }

        public bool RemoveCharacter(int index)
        {
            var toremove = _context.Characters.Find(index);
            var result = RemoveIfExists(toremove);
            if (result)
            {
                Console.WriteLine($"Removed character at index {index}");
            }

            return result;
        }

        public bool RemoveCharacter(string charname)
        {
            var toremove = _context.Characters.FirstOrDefault(x => x.Name == charname);
            var result = RemoveIfExists(toremove);
            if (result)
            {
                Console.WriteLine($"Removed character, {charname}");
            }
            return result;
        }

        /// <summary>
        /// Helper method to remove a character if exists
        /// </summary>
        /// <param name="c">The character to remove, or null.</param>
        /// <returns>Bool indicating if it was removed.</returns>
        private bool RemoveIfExists(Character c)
        {
            if (c != null)
            {
                _context.Characters.Remove(c);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Domain.Character GetCharacter(string name)
        {
            var character = _context.Characters.FirstOrDefault(c => c.Name == name);

            if( character != null)
            {
                return character.Convert();
            } else
            {
                return null;
            }
        }
    }
}
