using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Entities.DBEntities
{
    public class Order
    {
        public int Id { get; set; }
        public virtual User Seller { get; set; }
        public virtual User Buyer { get; set; }
        public Lot @Lot { get; set; }
        public bool isCompleted { get; set; }
    }
}
