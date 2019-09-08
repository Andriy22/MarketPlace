using MarketPlace.Entities.DBEntities;
using MarketPlace.Helpers;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarketPlace.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DBContext _context;
        public RegisterController(UserManager<User> userManager, DBContext dBContext)
        {
            this._userManager = userManager;
            this._context = dBContext;
        }
        [HttpPost]
        // ACTION IS ASYNC
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                Thread.Sleep(2000);
                if (ViewModelsValidator.isValid(model))
                {
                    foreach (var el in _context.Users)
                        if (el.NickName.ToLower() == model.NickName.ToLower())
                            return BadRequest(new { msg = "Nickname is already taken!" });
                    //if (model.Email.Contains("Admin"))
                    //    return StatusCode(404, "Oops, Админ хуесос!");
                    var user = new User()
                    {
                        Email = model.Email,
                        NickName = model.NickName,
                        UserName = model.Email
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return Ok(new { msg = "Account created!" });
                    }
                    else
                    {
                        foreach (var el in result.Errors)
                            return BadRequest(new { msg = el.Description });
                    }
                }
                return BadRequest(new { msg = "Data is invalid!" });
            }
            catch (Exception)
            {
                return BadRequest(new { msg = "Server crash!" });
            }
        }
    }
}