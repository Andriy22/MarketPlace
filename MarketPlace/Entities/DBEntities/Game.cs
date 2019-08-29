using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Entities.DBEntities
{
    public class Game
    {
        public Game()
        {
            this.Categories = new HashSet<Category>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
