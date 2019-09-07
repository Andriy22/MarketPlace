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
        public string InvokeHubMethod()
        {
            return Context.ConnectionId;
        }
        [Authorize]
        public async Task sendMessage(string message)
        {
            var user = this._context.Users.FirstOrDefault(x => x.Id == this.Context.User.Identity.Name);
            await this.Clients.All.SendAsync("reciveMessage", user.NickName, message, "user", "null", DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
        }

    }
}
