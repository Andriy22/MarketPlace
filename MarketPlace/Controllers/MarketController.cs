using MarketPlace.Entities.DBEntities;
using MarketPlace.Hubs;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketPlace.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class MarketController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly DBContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        public MarketController(UserManager<User> userManager, DBContext dBContext, IHubContext<ChatHub> hubContext)
        {
            this._userManager = userManager;
            this._context = dBContext;
            this._hubContext = hubContext;
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
            foreach (var el in this._context.Lots.Include(x => x.category).Where(x => x.category.ID == id && x.isDeleted == false && x.isActive == true).ToList().OrderByDescending(x => x.lastUp))
            {
                lots.Add(new AllLots() { Id = el.ID, Name = el.Name, Price = el.Price, isActive = Convert.ToBoolean(el.isActive) });
            }
            return Ok(lots);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> changeStatusLot(int id, bool status)
        {
            var lot = await this._context.Lots.Include(x => x.Owner).FirstAsync(x => x.ID == id);
            if (lot == null)
                return BadRequest(new { msg = "Lot not found" });
            if (lot.Owner.Id != this.User.Identity.Name && this.User.IsInRole("Admin") == false)
                return BadRequest(new { msg = "Access denied!" });
            this._context.Lots.Include(x => x.Owner).FirstOrDefault(x => x.ID == lot.ID).isActive = status;
            this._context.SaveChanges();

            return Ok();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> deleteLot(int id)
        {
           var lot =  await this._context.Lots.Include(x=>x.Owner).FirstAsync(x => x.ID == id);
            if (lot == null)
                return BadRequest(new { msg = "Lot not found" });
            if (lot.Owner.Id != this.User.Identity.Name && this.User.IsInRole("Admin") == false)
                return BadRequest(new { msg = "Access denied!" });
            this._context.Lots.FirstOrDefault(x => x.ID == lot.ID).isDeleted = true;
            this._context.SaveChanges();

            return Ok();
        }
        [HttpGet]
        [Authorize]
        public IActionResult getMyLots(int id)
        {
            Thread.Sleep(2000);
            var lots = new HashSet<AllLots>();
            foreach (var el in this._context.Lots.Include(x => x.category).Include(x => x.Owner).Where(x => x.category.ID == id && x.Owner.Id == User.Identity.Name && x.isDeleted == false).ToList().OrderByDescending(x => x.lastUp))
            {
                lots.Add(new AllLots() { Id = el.ID, Name = el.Name, Price = el.Price });
            }
            return Ok(lots);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Buy(int id) {
            var user = _context.Users.FirstOrDefault(x => x.Id == User.Identity.Name);
            var lot = _context.Lots.Include(x => x.Owner).First(x => x.ID == id);

            if (user.Balance - lot.Price < 0)
                return BadRequest(new { msg = "You do not have enough money!" });
            if(lot.Owner.Id == user.Id)
                return BadRequest(new { msg = "You can't buy from yourself!" });
            var order = new Order()
            {
                Lot = lot,
                Seller = lot.Owner,
                Buyer = user,
                isCompleted = false,
                timeOpen = DateTime.Now,
                timeClosed = DateTime.Now,
            };
            _context.Users.FirstOrDefault(x => x.Id == User.Identity.Name).Balance -= lot.Price;
          
            this._context.Orders.Add(order);
            this._context.SaveChanges();
            _hubContext.Clients.User(order.Buyer.Id).SendAsync("getPurchases", this._context.Orders.Include(x => x.Buyer).Where(x => x.Buyer.Id == user.Id && x.isCompleted == false).ToList().Count);
            _hubContext.Clients.User(order.Seller.Id).SendAsync("getSales", this._context.Orders.Include(x => x.Buyer).Where(x => x.Seller.Id == order.Seller.Id && x.isCompleted == false).ToList().Count);
            return Ok();
        }
        [HttpGet]
        [Authorize]
        public IActionResult ConfirmBuy(int id)
        {
            var order = _context.Orders.Include(x=>x.Lot).Include(x=>x.Seller).Include(x=>x.Buyer).FirstOrDefault(x => x.Id == id);
            if (order == null)
                return BadRequest(new { msg = "Order not found" });
            if (order.Buyer.Id != User.Identity.Name)
                return BadRequest(new { msg = "Access denied!" });
            _context.Users.FirstOrDefault(x => x.Id == order.Seller.Id).Balance += order.Lot.Price;
            _context.Orders.FirstOrDefault(x => x.Id == order.Id).isCompleted = true;
            _context.Orders.FirstOrDefault(x => x.Id == order.Id).timeClosed = DateTime.Now;
            _context.SaveChanges();
            _hubContext.Clients.User(order.Buyer.Id).SendAsync("getPurchases", this._context.Orders.Include(x => x.Buyer).Where(x => x.Buyer.Id == order.Buyer.Id && x.isCompleted == false).ToList().Count);
            _hubContext.Clients.User(order.Seller.Id).SendAsync("getSales", this._context.Orders.Include(x => x.Buyer).Where(x => x.Seller.Id == order.Seller.Id && x.isCompleted == false).ToList().Count);
            return Ok();
        }
        [HttpGet]
        [Authorize]
        public IActionResult ReturnMoney(int id)
        {
            var order = _context.Orders.Include(x => x.Lot).Include(x => x.Seller).Include(x => x.Buyer).FirstOrDefault(x => x.Id == id);
            if (order == null)
                return BadRequest(new { msg = "Order not found" });
            if (order.Seller.Id != User.Identity.Name)
                return BadRequest(new { msg = "Access denied!" });
            _context.Users.FirstOrDefault(x => x.Id == order.Buyer.Id).Balance += order.Lot.Price;
            _context.Orders.FirstOrDefault(x => x.Id == order.Id).isCompleted = true;
            _context.Orders.FirstOrDefault(x => x.Id == order.Id).timeClosed = DateTime.Now;
            _context.SaveChanges();
            _hubContext.Clients.User(order.Buyer.Id).SendAsync("getPurchases", this._context.Orders.Include(x => x.Buyer).Where(x => x.Buyer.Id == order.Buyer.Id && x.isCompleted == false).ToList().Count);
            _hubContext.Clients.User(order.Seller.Id).SendAsync("getSales", this._context.Orders.Include(x => x.Seller).Where(x => x.Seller.Id == order.Seller.Id && x.isCompleted == false).ToList().Count);
            return Ok();
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
        public IActionResult getmySales()
        {
            var orders = this._context.Orders.Include(x => x.Seller).Include(x => x.Buyer).Include(x => x.Lot).Include(x => x.Lot.category).Where(x=>x.Seller.Id == User.Identity.Name).ToList();
            if(orders != null)
            {
                var result = new HashSet<OrderViewModel>();
                foreach(var el in orders)
                {
                    result.Add(new OrderViewModel()
                    {
                        saller = el.Seller.NickName,
                        buyer = el.Buyer.NickName,
                        category = el.Lot.category.Name,
                        id = el.Id,
                        price = el.Lot.Price,
                        timeClosed = el.timeClosed.ToString("MM/dd/yyyy HH:mm"),
                        timeOpen = el.timeOpen.ToString("MM/dd/yyyy HH:mm"),
                        isCompleted = el.isCompleted,
                        name = el.Lot.Name
                    });
                }
                return Ok(result.OrderByDescending(x=>x.isCompleted));
            }
            return BadRequest(new { msg = "orders is null" });
        }

        [HttpGet]
        [Authorize]
        public IActionResult getOrder(int id )
        {

            var el = this._context.Orders.Include(x => x.Seller).Include(x => x.Buyer).Include(x => x.Lot).Include(x => x.Lot.category).FirstOrDefault(x=>x.Id == id);
            if(el != null)
            {
                var result = new OrderViewModel()
                {
                    saller = el.Seller.NickName,
                    buyer = el.Buyer.NickName,
                    category = el.Lot.category.Name,
                    id = el.Id,
                    price = el.Lot.Price,
                    timeClosed = el.timeClosed.ToString("MM/dd/yyyy HH:mm"),
                    timeOpen = el.timeOpen.ToString("MM/dd/yyyy HH:mm"),
                    isCompleted = el.isCompleted,
                    name = el.Lot.Name
                };
                return Ok(result);
            }
            return BadRequest(new { msg = "Lot not found" });
        }

        [HttpGet]
        [Authorize]
        public IActionResult getmyBuys()
        {
            var orders = this._context.Orders.Include(x => x.Seller).Include(x => x.Buyer).Include(x => x.Lot).Include(x => x.Lot.category).Where(x=>x.Buyer.Id == User.Identity.Name).ToList();
            if (orders != null)
            {
                var result = new HashSet<OrderViewModel>();
                foreach (var el in orders)
                {
                    result.Add(new OrderViewModel()
                    {
                        saller = el.Seller.NickName,
                        buyer = el.Buyer.NickName,
                        category = el.Lot.category.Name,
                        id = el.Id,
                        price = el.Lot.Price,
                        timeClosed = el.timeClosed.ToString("MM/dd/yyyy HH:mm"),
                        timeOpen = el.timeOpen.ToString("MM/dd/yyyy HH:mm"),
                        isCompleted = el.isCompleted,
                        name = el.Lot.Name
                    });
                }
                return Ok(result.OrderByDescending(x => x.isCompleted));
            }
            return BadRequest(new { msg = "orders is null" });
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
            var lot = this._context.Lots.Include(x => x.category).Include(x => x.Owner).Include(x => x.category).FirstOrDefault(x => x.ID == id && x.isDeleted == false);
            if (lot == null)
                return BadRequest(new { msg = "Lot not found!" });
            LotModel data = new LotModel()
            {
                id = lot.ID,
                Category = lot.category.Name,
                Game = "None",
                Price = lot.Price,
                Name = lot.Name,
                UserName = lot.Owner.NickName,
                Description = lot.Description,
                isActive = lot.isActive,
            };
            return Ok(data);
        }


    }
}