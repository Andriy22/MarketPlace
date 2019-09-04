using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Entities.DBEntities
{
    public class Lot
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string  Description { get; set; }
        public decimal Price { get; set; }
        public DateTime lastUp { get; set; }
        public virtual Category category { get; set; }
        public virtual User Owner { get; set; }
    }
}
