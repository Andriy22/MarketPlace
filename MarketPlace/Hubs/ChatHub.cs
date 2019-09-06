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
        [Authorize]
        public async Task Send(string message)
        {
           await this.Clients.User(Context.User.Identity.Name).SendAsync("sendBalance", 999);
        }
        public string InvokeHubMethod()
        {
            return Context.ConnectionId;
        }
      
        //public override Task OnConnectedAsync()
        //{
        //    var ctx = Context;
        //    string name = Context.User.Identity.Name;
        //    if (!string.IsNullOrEmpty(name))
        //    {
               
        //        Clients.All.SendAsync("sendBalance", 229);
        //    } else
        //    {
        //        Clients.All.SendAsync("sendBalance", 228);
        //    }
           
        //    return base.OnConnectedAsync();
        //}
    }
}
