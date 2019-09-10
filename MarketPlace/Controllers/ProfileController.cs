using MarketPlace.Entities.DBEntities;
using MarketPlace.Hubs;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading;

namespace MarketPlace.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DBContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        public ProfileController(UserManager<User> userManager, DBContext dBContext, IHubContext<ChatHub> hubContext)
        {
            this._userManager = userManager;
            this._context = dBContext;
            this._hubContext = hubContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult addBalance(string code)
        {
            Thread.Sleep(2000);
            this._context.Users.FirstOrDefault(x => x.Id == User.Identity.Name).Balance += 500;
            try
            {
                this._context.SaveChanges();
                this._hubContext.Clients.User(User.Identity.Name).SendAsync("sendBalance", this._context.Users.FirstOrDefault(x => x.Id == User.Identity.Name).Balance);
                return Ok(500);
            }
            catch
            {
                return BadRequest(new { msg = "Unknow error!" });
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult myBalance()
        {
            var balance = this._context.Users.FirstOrDefault(x => x.Id == User.Identity.Name).Balance;
            return Ok(balance);
        }

        [HttpPost]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> changePassword([FromBody] ChangePasswordModel model)
        {
            var user = _userManager.FindByIdAsync(User.Identity.Name).Result;
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.currentPassword, model.newPassword);
                if (result.Succeeded)
                {
                    return Ok(new { msg = "Password changed!" });
                }

                return BadRequest(new { msg = "Password is invalid" });
            }
            return BadRequest(new { msg = "User not found" });
        }
    }
}