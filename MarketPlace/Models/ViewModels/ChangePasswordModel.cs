using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.ViewModels
{
    public class ChangePasswordModel
    {
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
    }
}
