using MarketPlace.Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.ServerModels
{
    public class AuthOject
    {
        public User user { get; set; }
        public bool isAuth { get; set; }
    }
}
