using System;
using System.Collections.Generic;

#nullable disable

namespace ShipBot.DataAccsess
{
    public partial class ShipRating
    {
        public long Ship { get; set; }
        public string Rater { get; set; }
        public long Rating { get; set; }

        public virtual Owner RaterNavigation { get; set; }
        public virtual Ship ShipNavigation { get; set; }
    }
}
