using MarketPlace.Entities.DBEntities;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace MarketPlace.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class MarketController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly DBContext _context;
        //private readonly IHubContext<ChatHub> _hubContext;
        public MarketController(UserManager<User> userManager, DBContext dBContext)
        {
            this._userManager = userManager;
            this._context = dBContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult getGames()
        {

            Thread.Sleep(2000);
            var games = new HashSet<AllGamesViewModel>();
            foreach (var el in this._context.Games.Include(x => x.Categories))
            {
                var categories = new HashSet<CategoryViewModel>();
                foreach (var c in el.Categories)
                    categories.Add(new CategoryViewModel() { Id = c.ID.ToString(), Name = c.Name });
                games.Add(new AllGamesViewModel { Categories = categories, Game = el.Name, ID = el.ID });
                //categories.Clear();
            }
            return Ok(games);

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getLots(int id)
        {
            Thread.Sleep(2000);
            var lots = new HashSet<AllLots>();
            foreach (var el in this._context.Lots.Include(x => x.category).Where(x => x.category.ID == id).ToList().OrderByDescending(x => x.lastUp))
            {
                lots.Add(new AllLots() { Id = el.ID, Name = el.Name, Price = el.Price, isActive = Convert.ToBoolean(el.isActive) });
            }
            return Ok(lots);
        }



        [HttpGet]
        [Authorize]
        public IActionResult getMyLots(int id)
        {
            Thread.Sleep(2000);
            var lots = new HashSet<AllLots>();
            foreach (var el in this._context.Lots.Include(x => x.category).Include(x => x.Owner).Where(x => x.category.ID == id && x.Owner.Id == User.Identity.Name).ToList().OrderByDescending(x => x.lastUp))
            {
                lots.Add(new AllLots() { Id = el.ID, Name = el.Name, Price = el.Price });
            }
            return Ok(lots);
        }

        [HttpPost]
        [Authorize]
        public IActionResult newLot(NewLotViewModel model)
        {
            var category = this._context.Categories.Include(x => x.Game).FirstOrDefault(x => x.ID == model.Category);
            var user = _context.Users.First(x => x.Id == User.Identity.Name);
            if (category == null && user == null)
                return BadRequest(new { msg = "Category or User not found" });
            this._context.Lots.Add(new Lot()
            {
                category = category,
                Price = model.Price,
                Name = model.Name,
                Description = model.Description,
                Owner = user,
                lastUp = DateTime.Now,
                isActive = true
            });

            try
            {
                this._context.SaveChanges();
                return Ok(new { msg = "added" });
            }
            catch (Exception e)
            {
                return BadRequest(new { msg = e.Message });
            }

        }

        [HttpGet]
        [Authorize]
        public IActionResult upLots(int id)
        {
            Thread.Sleep(2000);
            foreach (var el in this._context.Lots.Include(x => x.Owner).Include(x => x.category).Where(x => x.category.ID == id && x.Owner.Id == User.Identity.Name).ToList())
            {
                if (el.lastUp > DateTime.Now)
                    return BadRequest(new { msg = "Wait " + Convert.ToInt32((el.lastUp - DateTime.Now).TotalMinutes).ToString() + " minutes!" });
                el.lastUp = DateTime.Now.AddHours(2);
            }
            try
            {
                this._context.SaveChanges();
                return Ok(new { msg = "Lots upped" });
            }
            catch (Exception)
            {
                return BadRequest(new { msg = "Unknow error" });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult getLot(int id)
        {
            var lot = this._context.Lots.Include(x => x.category).Include(x => x.Owner).Include(x => x.category).FirstOrDefault(x => x.ID == id);

            LotModel data = new LotModel()
            {
                Category = lot.category.Name,
                Game = "None",
                Price = lot.Price,
                Name = lot.Name,
                UserName = lot.Owner.NickName,
                Description = lot.Description
            };
            return Ok(data);
        }


    }
}