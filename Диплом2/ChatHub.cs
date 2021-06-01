using Microsoft.AspNetCore.SignalR;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Диплом2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Диплом2
{
    public class ChatHub : Hub
    {
       
        public async Task Send(string message, string username, int idsender, int idtaker)
        {
           
            AppDBContext context = new AppDBContext();
            Chat mes = new Chat();
            mes.IdSender = idsender;
            mes.IdWorker = idtaker;
            mes.TextLetter = message;
            mes.Time = DateTime.Now.ToString();
            await context.Chat.AddAsync(mes);
            await  context.SaveChangesAsync();
            await this.Clients.All.SendAsync("Send", message, username);
        }
        
    }
}
