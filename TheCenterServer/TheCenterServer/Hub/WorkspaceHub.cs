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

        public List<BoardUI> GetBoards(string workspace)
        {
            var space = ModuleManager.Ins.WorkspaceManager.Get(workspace);
            var list = new List<BoardUI>();
            for (int i = 0; i < space.desc.Boards.Count; i++)
            {
                list.Add(BoardUI.From(space.desc.Boards[i], 
                    space.modules.Find(m => m.ID == space.desc.Boards[i].Id).BuildInterface()));
            }
            return list;
        }

        public List<WorkspaceDesc> GetWorkspaces()
        {
            return ModuleManager.Ins.WorkspaceManager.Workspaces.Select(w => w.desc).ToList();
        }

        public bool CreateWorkspace(string name)
        {
            ModuleManager.Ins.WorkspaceManager.Create(name);
            return true;
        }

        public bool CreateBoard(string wkspace, string bdtype)
        {
            try
            {
                ModuleManager.Ins.WorkspaceManager.Get(wkspace).CreateBoard(bdtype);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
