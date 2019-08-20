using MarketPlace.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Helpers
{
    public static class ViewModelsValidator
    {
        static public bool isValid(RegisterViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.NickName) && !string.IsNullOrEmpty(model.Password))
                return true;
            return false;
        }
    }
}
