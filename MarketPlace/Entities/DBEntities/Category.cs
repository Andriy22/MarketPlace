using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Entities.DBEntities
{
    public class Category
    {
        public Category()
        {
            this.Lots = new HashSet<Lot>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Lot> Lots { get; set; }
        public virtual Game @Game { get; set; }
    }
}
