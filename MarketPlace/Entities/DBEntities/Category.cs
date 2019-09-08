using System.Collections.Generic;

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
