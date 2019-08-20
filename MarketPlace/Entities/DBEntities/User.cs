using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Entities.DBEntities
{
    public class User : IdentityUser
    {
        public string NickName { get; set; }
        public decimal Balance { get; set; }

    }
}
