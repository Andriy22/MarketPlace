﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MarketPlace.JWT
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "https://localhost:44362/"; // потребитель токена
        const string KEY = "SuperPuperSecretKey!228";   // ключ для шифрации
        public const int LIFETIME = 1440; // время жизни токена - 120 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
