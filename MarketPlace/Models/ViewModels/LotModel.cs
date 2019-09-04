using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.ViewModels
{
    public class LotModel
    {
        public string UserName { get; set; }
        public string Category { get; set; }
        public string Game { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
