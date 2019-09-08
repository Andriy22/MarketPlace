using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Entities.DBEntities
{
    public class User : IdentityUser
    {
        public string NickName { get; set; }
        public decimal Balance { get; set; }

    }
}
