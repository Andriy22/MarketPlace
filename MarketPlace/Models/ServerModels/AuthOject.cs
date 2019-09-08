using MarketPlace.Entities.DBEntities;

namespace MarketPlace.Models.ServerModels
{
    public class AuthOject
    {
        public User user { get; set; }
        public bool isAuth { get; set; }
    }
}
