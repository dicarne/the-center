using System;
using System.Collections.Generic;
using TheCenterServer.PModule;

namespace TheCenterServer
{
    public class WorkspaceManager
    {
        public List<Workspace> Workspaces = new();

        /// <summary>
        /// 创建一个新的工作空间
        /// </summary>
        /// <param name="workname"></param>
        /// <returns></returns>
        public Workspace Create(string workname)
        {
            var newwk = new Workspace()
            {
                desc = new WorkspaceDesc()
                {
                    WName = workname,
                    Id = Guid.NewGuid().ToString(),
                    // TODO 模板
                    Boards = new List<BoardDesc>()
                }
            };
            Workspaces.Add(newwk);
            newwk.Init();
            return newwk;
        }

        public void Delete(string workid)
        {
            var index = Workspaces.FindIndex(w => w.desc.Id == workid);
            if (index != -1) Workspaces.RemoveAt(index);
        }

        public Workspace Get(string workid)
        {
            return Workspaces.Find(w => w.desc.Id == workid);
        }
    }

    public class Workspace
    {
        public WorkspaceDesc desc;
        public List<ModuleBase> modules = new();
        public void Init()
        {
            foreach (var b in desc.Boards)
            {
                // 根据类型创建对应模块实例
                var m = ModuleManager.Ins.BuildFrom(b.CardType);
                if (string.IsNullOrEmpty(b.Id))
                {
                    b.Id = Guid.NewGuid().ToString();
                }
                m.ID = b.Id;
                modules.Add(m);
            }
        }

        public void CreateBoard(string type)
        {
            var m = ModuleManager.Ins.BuildFrom(type);
            m.ID = Guid.NewGuid().ToString();
            modules.Add(m);
            desc.Boards.Add(new BoardDesc() { CardType = type, Id = m.ID });
        }
    }
}