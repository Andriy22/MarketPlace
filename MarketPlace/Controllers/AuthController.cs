using MarketPlace.Entities.DBEntities;
using MarketPlace.JWT;
using MarketPlace.Models;
using MarketPlace.Models.ServerModels;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace MarketPlace.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DBContext _context;
        public AuthController(UserManager<User> userManager, DBContext dBContext)
        {
            this._userManager = userManager;
            this._context = dBContext;
        }
        //[HttpPost]
        // ACTION IS ASYNC
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    try
        //    {
        //        if (ViewModelsValidator.isValid(model))
        //        {
        //            foreach (var el in _context.Users)
        //                if (el.NickName.ToLower() == model.NickName.ToLower())
        //                    return BadRequest(new { msg = "Nickname is already taken!" });
        //            //if (model.Email.Contains("Admin"))
        //            //    return StatusCode(404, "Oops, Админ LOX!");
        //            var user = new User() { Email = model.Email,
        //                                    NickName = model.NickName,
        //                                    UserName = model.Email };
        //            var result = await _userManager.CreateAsync(user, model.Password);
        //            if (result.Succeeded) {
        //                return Ok(new { msg = "Account created!" });
        //            }
        //            else
        //            {
        //                foreach (var el in result.Errors)
        //                    return BadRequest(new { msg = el.Description});
        //            }   
        //        }
        //        return BadRequest(new { msg = "Data is invalid!" });
        //    }
        //    catch(Exception)
        //    {
        //        return BadRequest(new { msg = "Server crash!" });
        //    }
        //}







        [HttpPost("/Auth")]
        public IActionResult Auth(AuthViewModel model)
        {
            Thread.Sleep(2000);
            var identity = GetIdentity(model.Email, model.Password);
            if (identity == null)
            {
                return BadRequest(new { msg = "Login or password is invalid" });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var user = getUserAsync(model.Email, model.Password);
            var response = new
            {
                access_token = encodedJwt,
                nickname = user.user.NickName,
                roles = _userManager.GetRolesAsync(user.user).Result.ToList(),
            };

            // сериализация ответа
            return Ok(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
        private AuthOject getUserAsync(string username, string password)
        {
            var obj = new AuthOject();
            User person = this._userManager.FindByEmailAsync(username).Result;
            if (person != null)
            {
                obj.user = person;
                var isAuth = this._userManager.CheckPasswordAsync(person, password).Result;
                if (isAuth)
                {
                    obj.isAuth = true;
                }
            }
            return obj;
        }
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var result = getUserAsync(username, password);
            if (result.isAuth)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, result.user.Id),
                   //new Claim(ClaimsIdentity.DefaultRoleClaimType, "User"),
                };
                var roles = _userManager.GetRolesAsync(result.user).Result.ToList();
                foreach (var el in roles)
                {
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, el));
                }
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }



}
