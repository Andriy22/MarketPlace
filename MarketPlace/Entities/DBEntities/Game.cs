using System.Collections.Generic;

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
