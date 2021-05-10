using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TheCenterServer
{
    public class WorkspaceHub : Hub
    {
        public WorkspaceHub()
        {
            Ins = this;
        }
        public static WorkspaceHub Ins { get; set; }
        public async Task SendMessageToBoard(string workspace, string boardID, string data)
        {
            await Clients.All.SendAsync("ReceiveBoard", workspace, boardID, data);
        }

        public async override Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId);
        }

        public async override Task OnDisconnectedAsync(Exception? e)
        {
            focusWorkspace.Remove(Context.ConnectionId);
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
            Console.WriteLine(JsonSerializer.Serialize(list));
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

        public class HandleEventArg
        {
            public string wk { get; set; }
            public string bd { get; set; }
            public string ui { get; set; }
            public string e { get; set; }
            public string[] arg { get; set; }
        }
        public object HandleEvent(string argstr)
        {
            HandleEventArg arg = JsonSerializer.Deserialize<HandleEventArg>(argstr);
            var module = ModuleManager.Ins.WorkspaceManager.Get(arg.wk).modules.Find(b => b.ID == arg.bd);
            var res = module.HandleUIEvent(arg.ui, arg.e, arg.arg);
            return res;
        }

        public void FocusWorkspace(string workspaceid)
        {
            if (focusWorkspace.TryGetValue(Context.ConnectionId, out var old))
            {
                old.ConnectID = null;
            }
            var sp = ModuleManager.Ins.WorkspaceManager.Get(workspaceid);
            sp.ConnectID = Context.ConnectionId;
            focusWorkspace.Add(Context.ConnectionId, sp);
        }

        Dictionary<string, Workspace> focusWorkspace = new Dictionary<string, Workspace>();

        public void SendUIToClient(string connectID, string workspace, string board, object data)
        {
            Clients.Client(connectID).SendAsync("HandleServer", workspace, board, JsonSerializer.Serialize(data, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    }
}
