using System;

namespace MarketPlace.Entities.DBEntities
{
    public class Lot
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime lastUp { get; set; }
        public bool isActive { get; set; }
        public virtual Category category { get; set; }
        public virtual User Owner { get; set; }
        public bool isDeleted { get; set; }
    }
}
