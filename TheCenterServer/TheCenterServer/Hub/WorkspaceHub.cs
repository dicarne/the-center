using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TheCenterServer
{
    public class WorkspaceHub: Hub
    {
        public async Task SendMessageToBoard(string workspace, string boardID, string data)
        {
            await Clients.All.SendAsync("ReceiveBoard", workspace, boardID, data);
        }

        public async override Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId);
        }
    }
}
