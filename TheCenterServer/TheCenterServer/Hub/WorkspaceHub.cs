using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TheCenterServer
{
    public class WorkspaceHub : Hub
    {
        Dictionary<string, string> Current = new Dictionary<string, string>();
        public async Task SendMessageToBoard(string workspace, string boardID, string data)
        {
            await Clients.All.SendAsync("ReceiveBoard", workspace, boardID, data);
        }

        public async override Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId);
            Current.Add(Context.ConnectionId, "");
        }

        public async override Task OnDisconnectedAsync(Exception? e)
        {
            Console.WriteLine($"ID {Context.ConnectionId}, {e}");
            Current.Remove(Context.ConnectionId);
        }

        public async Task<string> SayHello()
        {
            return "Hello SingalR!";
        }

        public async Task<List<BoardDesc>> GetBoards(string workspace)
        {
            return new List<BoardDesc>(){
                new BoardDesc(){
                    CName="Card1",
                    CardType="C1",
                    W=8,
                    Id="1"
                },
                 new BoardDesc(){
                    CName="Card2",
                    CardType="C2",
                    W=8,
                    Id="2"
                }
            };
        }
    }
}
