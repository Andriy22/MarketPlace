using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.ViewModels
{
    public class ChatMsgViewModel
    {
        public string nickname { get; set; }
        public string message { get; set; }
        public string role { get; set; }
        public string ava { get; set; }
        public string time { get; set; }
        public string toname { get; set; }
    }
}
