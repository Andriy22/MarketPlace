//using MarketPlace.Entities.DBEntities;
using MarketPlace.Entities.DBEntities;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly DBContext _context;
        //private readonly IHubContext<ChatHub> _hubContext;
        public ChatHub(UserManager<User> userManager, DBContext dBContext)
        {
            this._userManager = userManager;
            this._context = dBContext;
        }
        [Authorize]
        public async Task Send(string message)
        {
            await this.Clients.User(Context.User.Identity.Name).SendAsync("sendBalance", this._context.Users.FirstOrDefault(x=>x.Id == Context.User.Identity.Name).Balance);
        }



        public async Task SendMessageToUser(string username, string message)
        {
            var to = this._context.Users.First(x => x.NickName == username);
            var sender = this._context.Users.FirstOrDefault(x => x.Id == this.Context.User.Identity.Name);
            if (to != null)
            {
                var msg = new ChatMsgViewModel()
                {
                    nickname = sender.NickName,
                    ava = "null",
                    role = "User",
                    message = message,
                    time = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                    toname = to.NickName
                };
                this._context.ChatMassages.Add(new ChatMassage
                {
                    Message = message,
                    Sender = sender,
                    To = to,
                    Room = null,
                    Time = DateTime.Now
                });
                this._context.SaveChanges();
                await this.Clients.Caller.SendAsync("reciveMessegeFromUser", msg);
                await this.Clients.User(to.Id).SendAsync("reciveMessegeFromUser", msg);
            }
            else
            {
                var msg = new ChatMsgViewModel()
                {
                    nickname = "Notify",
                    ava = "null",
                    role = "Notify",
                    message = "User not found",
                    time = DateTime.Now.ToString("MM/dd/yyyy HH:mm")
                };
                await this.Clients.Caller.SendAsync("reciveMessegeFromUser", msg);
            }
        }
        public async Task SendAllMessages(int category)
        {
            var room = "global";
            if (category > 0)
                room = this._context.Categories.Include(x => x.Game).FirstOrDefault(x => x.ID == category).Game.Name;
            var msgs = this._context.ChatMassages.Include(x => x.Sender).Include(x => x.To).Where(x => x.Time.AddDays(1) >= DateTime.Now && x.To == null && x.Room == room);
            var result = new HashSet<ChatMsgViewModel>();
            foreach (var el in msgs)
            {
                result.Add(new ChatMsgViewModel()
                {
                    nickname = el.Sender.NickName,
                    ava = "null",
                    role = "User",
                    message = el.Message,
                    time = el.Time.ToString("MM/dd/yyyy HH:mm"),
                });
            }
            await this.Clients.Group(room).SendAsync("reciveAllMessages", result);
            result.Clear();
        }
        public async Task GetUsersForSendMeMsg()
        {
            var msgs = this._context.ChatMassages.Include(x => x.Sender).Include(x => x.To).Where(x => (x.To.Id == this.Context.User.Identity.Name || x.Sender.Id == this.Context.User.Identity.Name) && (x.Sender != null && x.To != null && x.Room == null)).ToList();
            var users = new HashSet<string>();
            foreach (var el in msgs)
            {
                if (el.Sender.Id == this.Context.User.Identity.Name)
                    users.Add(el.To.NickName);
                else
                    users.Add(el.Sender.NickName);
            }
            users.Distinct();
            await this.Clients.Caller.SendAsync("getUsersForSendMe", users);
        }
        public async Task GetMessagesToMe(string username)
        {
            var myId = this.Context.User.Identity.Name;
            var messages = this._context.ChatMassages.Include(x => x.Sender).Include(x => x.To).Where(x => (x.Sender.Id == myId && x.To.NickName == username) || (x.Sender.NickName == username && x.To.Id == myId) && x.Room == null);
            var result = new HashSet<ChatMsgViewModel>();
            foreach (var el in messages)
            {
                result.Add(new ChatMsgViewModel()
                {
                    nickname = el.Sender.NickName,
                    ava = "null",
                    role = "User",
                    message = el.Message,
                    time = el.Time.ToString("MM/dd/yyyy HH:mm"),
                    toname = el.To.NickName
                });
            }
            await Clients.Caller.SendAsync("reciveMessegesUser", result);
        }
        //[Authorize]
        public async Task SwitchGroup(int category)
        {
            var room = "global";
            if (category > 0)
            {
                await this.Groups.RemoveFromGroupAsync(Context.ConnectionId, "global");
                room = this._context.Categories.Include(x => x.Game).FirstOrDefault(x => x.ID == category).Game.Name;
            }
            await this.Groups.AddToGroupAsync(Context.ConnectionId, room);

        }
        [Authorize]
        public async Task sendMessage(string message, int category)
        {
            var room = "global";
            if (category > 0)
                room = this._context.Categories.Include(x => x.Game).FirstOrDefault(x => x.ID == category).Game.Name;
            var user = this._context.Users.FirstOrDefault(x => x.Id == this.Context.User.Identity.Name);
            this._context.ChatMassages.Add(new ChatMassage()
            {
                Sender = user,
                Message = message,
                Time = DateTime.Now,
                To = null,
                Room = room
            });
            this._context.SaveChanges();
            await this.Clients.Group(room).SendAsync("reciveMessage", new ChatMsgViewModel()
            {

                nickname = user.NickName,
                ava = "null",
                role = "User",
                message = message,
                time = DateTime.Now.ToString("MM/dd/yyyy HH:mm")

            });
        }
        public async Task getPursh()
        {
            await Clients.Caller.SendAsync("getPurchases", this._context.Orders.Include(x => x.Buyer).Where(x => x.Buyer.Id == Context.User.Identity.Name && x.isCompleted == false).ToList().Count);
        }
        public async Task getSales() {
            await Clients.Caller.SendAsync("getSales", this._context.Orders.Include(x => x.Seller).Where(x => x.Seller.Id == Context.User.Identity.Name && x.isCompleted == false).ToList().Count);
        }
        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(this.Context.ConnectionId, "global");
            return base.OnConnectedAsync();
        }


    }
}
