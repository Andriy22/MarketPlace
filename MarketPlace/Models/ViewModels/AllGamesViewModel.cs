using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.ViewModels
{
    public class AllGamesViewModel
    {
        public AllGamesViewModel()
        {
            this.Categories = new HashSet<CategoryViewModel>();
        }
        public int ID { get; set; }
        public string Game { get; set; }
        public ICollection<CategoryViewModel> Categories { get; set; }
    }
    public class CategoryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}