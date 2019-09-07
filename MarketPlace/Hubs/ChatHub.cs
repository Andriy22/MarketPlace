//using MarketPlace.Entities.DBEntities;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Entities.DBEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MarketPlace.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

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
           await this.Clients.User(Context.User.Identity.Name).SendAsync("sendBalance", 999);
        }




        public async Task SendAllMessages(int category)
        {
            var room = "global";
            if (category > 0)
                room = this._context.Categories.Include(x => x.Game).FirstOrDefault(x => x.ID == category).Game.Name;
            var msgs = this._context.ChatMassages.Include(x => x.Sender).Include(x => x.To).Where(x => x.Time.AddDays(1) >= DateTime.Now && x.To == null && x.Room == room);
            var result = new HashSet<ChatMsgViewModel>();
            foreach(var el in msgs)
            {
                result.Add(new ChatMsgViewModel()
                {
                    nickname = el.Sender.NickName,
                    ava = "null",
                    role = "User",
                    message = el.Message,
                    time = el.Time.ToString("MM/dd/yyyy HH:mm")
                });
            }
            await this.Clients.Group(room).SendAsync("reciveAllMessages", result);
            result.Clear();
        }

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
                role = "Admin",
                message = message,
                time = DateTime.Now.ToString("MM/dd/yyyy HH:mm")

            });
        }




        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(this.Context.ConnectionId, "global");
            return base.OnConnectedAsync();
        }


    }
}
