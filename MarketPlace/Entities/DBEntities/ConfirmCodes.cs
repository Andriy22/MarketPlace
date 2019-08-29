using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Entities.DBEntities
{
    public class ConfirmCodes
    {
        public int ID { get; set; }
        public virtual User user { get; set; }
        public int code { get; set; }
    }
}
