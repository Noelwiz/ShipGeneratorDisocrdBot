using System;
using System.Collections.Generic;

#nullable disable

namespace ShipBot.DataAccsess
{
    public partial class Ship
    {
        public Ship()
        {
            ShipRatings = new HashSet<ShipRating>();
        }

        public long Id { get; set; }
        public long? Character1 { get; set; }
        public long? Character2 { get; set; }

        public virtual Character Character1Navigation { get; set; }
        public virtual Character Character2Navigation { get; set; }
        public virtual ICollection<ShipRating> ShipRatings { get; set; }
    }
}
