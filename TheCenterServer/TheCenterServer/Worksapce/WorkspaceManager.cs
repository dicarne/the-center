using System;
using System.Collections.Generic;
using System.Linq;
using TheCenterServer.PModule;
using LiteDB;

namespace TheCenterServer
{
    public class WorkspaceManager: IDisposable
    {
        public static string DBPath = @"D://test.db";
        public static LiteDatabase DB;
        public List<Workspace> Workspaces = new();

        public WorkspaceManager()
        {
            DB = new LiteDatabase(DBPath);
        }

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
            Save();
            return newwk;
        }

        public void Delete(string workid)
        {
            var index = Workspaces.FindIndex(w => w.desc.Id == workid);
            if (index != -1) Workspaces.RemoveAt(index);
            Save();
        }

        public void Dispose()
        {
            DB.Dispose();
        }

        public Workspace Get(string workid)
        {
            return Workspaces.Find(w => w.desc.Id == workid);
        }

        public void Recovery()
        {
            var db = DB;
            var col = db.GetCollection<WorkspaceSaveData>("workspace");
            col.EnsureIndex(d => d.name);
            var old = col.FindOne(w => w.name == "main");
            if(old != null)
            {
                Workspaces = old.workspaces.Select(d =>
                {
                    try
                    {
                        var w = new Workspace();
                        w.desc = d;
                        w.Init();
                        return w;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return new Workspace() { desc = d };
                    }
                    
                }).ToList();
            }
        }

        public void Save()
        {
            var list = Workspaces.Select(w => w.desc).ToList();
            var db = DB;
            var col = db.GetCollection<WorkspaceSaveData>("workspace");
            col.EnsureIndex(d => d.name);
            var old = col.FindOne(w => w.name == "main");
            if(old == null)
            {
                col.Insert(new WorkspaceSaveData() { name = "main", workspaces = list });
            }
            else
            {
                old.workspaces = list;
                try
                {
                    col.Update(old);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
        }
        class WorkspaceSaveData
        {
            public ObjectId _id { get; set; }
            public string name { get; set; }
            public List<WorkspaceDesc> workspaces { get; set; }
        }
    }

    public class Workspace
    {
        public WorkspaceDesc desc;
        public List<ModuleBase> modules;
        public string ConnectID { get; set; }
        public void Init()
        {
            modules = new();
            foreach (var b in desc.Boards)
            {
                // 根据类型创建对应模块实例
                var m = ModuleManager.Ins.BuildFrom(b.CardType);
                if (string.IsNullOrEmpty(b.Id))
                {
                    b.Id = Guid.NewGuid().ToString();
                }
                m.Workspace = this;
                m.ID = b.Id;

                modules.Add(m);
                m.Recovery();
                m.OnLoad();
            }
        }

        public void CreateBoard(string type)
        {
            var m = ModuleManager.Ins.BuildFrom(type);
            m.ID = Guid.NewGuid().ToString();
            modules.Add(m);
            m.Workspace = this;

            desc.Boards.Add(new BoardDesc() { CardType = type, Id = m.ID });
            m.OnFirstCreate();
            m.OnLoad();
            m.Save();
            ModuleManager.Ins.WorkspaceManager.Save();
        }
    }
}