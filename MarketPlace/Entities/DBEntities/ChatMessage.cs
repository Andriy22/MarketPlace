using System;

namespace MarketPlace.Entities.DBEntities
{
    public class ChatMassage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }

        public string Room { get; set; }

        public virtual User Sender { get; set; }
        public virtual User To { get; set; }
    }
}
