using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MarketPlace.Entities.DBEntities;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class MarketController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly DBContext _context;
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
            foreach(var el in this._context.Games.Include(x=>x.Categories)) {
                var categories = new HashSet<CategoryViewModel>();
                foreach (var c in el.Categories)
                    categories.Add(new CategoryViewModel() { Id = c.ID.ToString(), Name = c.Name });
                games.Add(new AllGamesViewModel { Categories = categories, Game = el.Name, ID = el.ID });
                //categories.Clear();
            }
            return Ok(games);

        } 
        


    }
}