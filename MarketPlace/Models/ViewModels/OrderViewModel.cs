using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.ViewModels
{
    public class OrderViewModel
    {
        public int id { get; set; }
        public string saller { get; set; }
        public string buyer { get; set; }
        public decimal price { get; set; }
        public string timeOpen { get; set; }
        public string timeClosed { get; set; }
        public string category { get; set; }
        public bool isCompleted { get; set; }
        public string name { get; set; }
    }
}
