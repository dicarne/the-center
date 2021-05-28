using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCenterServer.PModule;

namespace TheCenterServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PluginModuleController : ControllerBase
    {
        public class ValueWrapper
        {
            public string value { get; set; }
        }

        [HttpPost]
        public void SyncUI(string work, string board)
        {
            WorkspaceManager.Ins.Get(work).modules.Find(b => b.ID == board)!.SyncUI();
        }

        public class EnsureValueBody
        {
            public List<string[]> value { get; set; }
        }

        [HttpPost]
        public void EnsureValue(string work, string board, EnsureValueBody keybody)
        {
            var keys = keybody.value;
            var m = WorkspaceManager.Ins.Get(work).modules.Find(b => b.ID == board) as PluginModule;
            bool dirt = false;
            foreach (var k in keys)
            {
                if (!m.Data.ContainsKey(k[0]))
                {
                    
                    m.Data[k[0]] = k[1];
                    dirt = true;
                }
            }
            if(dirt) m.Save();
        }

        [HttpGet]
        public ValueWrapper? GetValue(string work, string board, string key)
        {
            var m = WorkspaceManager.Ins.Get(work).modules.Find(b => b.ID == board) as PluginModule;
            if (m.Data.TryGetValue(key, out var b))
            {
                return new()
                {
                    value = b
                };
            }
            Console.WriteLine("NOT FOUND " + key);
            return null;
        }

        [HttpPost]
        public void SetValue(string work, string board, string key, ValueWrapper value)
        {
            var m = WorkspaceManager.Ins.Get(work).modules.Find(b => b.ID == board) as PluginModule;
            m.Data[key] = value.value;
            m.Save();
        }

        [HttpGet]
        public BoardDesc GetBoardDesc(string work, string board)
        {
            var m = WorkspaceManager.Ins.Get(work).modules.Find(b => b.ID == board) as PluginModule;
            return m.BoardDesc;
        }

        [HttpGet]
        public Dictionary<string, string> GetGlobalVariable(string work)
        {
            return WorkspaceManager.Ins.Get(work).desc.globalVariables;
        }
    }
}
