using System;
using System.Collections.Generic;
using System.Linq;
using TheCenterServer.PModule;
using LiteDB;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.Json;

namespace TheCenterServer
{
    public class WorkspaceManager : IDisposable
    {
        public static string DBPath => Ins.config.DBPath;
        public static LiteDatabase DB;
        public List<Workspace> Workspaces = new();
        public static WorkspaceManager Ins;
        public class WorkspaceConfig
        {
            public string DBPath { get; set; } = @"D://test.db";
        }
        WorkspaceConfig config;

        readonly string configPath;
        public WorkspaceManager()
        {
            Ins = this;
            var appdata = Path.Combine(Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "AppData" : "Home")!, "TheCenter");
            if (!Directory.Exists(appdata)) Directory.CreateDirectory(appdata);
            var configp = Path.Combine(appdata, "config.json");
            configPath = configp;
            if (File.Exists(configp))
            {
                try
                {
                    config = System.Text.Json.JsonSerializer.Deserialize<WorkspaceConfig>(File.ReadAllText(configp))!;
                }
                catch (Exception e)
                {
                    config = new WorkspaceConfig()
                    {
                        DBPath = Path.Combine(appdata, "thecenterdb.db")
                    };
                    File.Move(configPath, configp + "." + DateTime.UtcNow.Ticks);
                    File.WriteAllText(configPath, System.Text.Json.JsonSerializer.Serialize(config));
                }
            }
            else
            {
                config = new WorkspaceConfig()
                {
                    DBPath = Path.Combine(appdata, "thecenterdb.db")
                };
                File.WriteAllText(configPath, System.Text.Json.JsonSerializer.Serialize(config));
            }

#if DEBUG
            config.DBPath = Path.Combine(appdata, "thecenterdb_debug.db");
#endif

            DB = new LiteDatabase(DBPath);
        }

        public void SetConfig(Action<WorkspaceConfig> modify)
        {
            var oldpath = config.DBPath;
            modify(config);
            
            File.WriteAllText(configPath, System.Text.Json.JsonSerializer.Serialize(config));
            if (config.DBPath != oldpath)
            {
                DB.Dispose();
                // 移动数据库
                if (!File.Exists(config.DBPath))
                {
                    File.Move(oldpath, config.DBPath);
                }
                DB = new LiteDatabase(DBPath);
            }
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
                    wName = workname,
                    id = Guid.NewGuid().ToString(),
                    // TODO 模板
                    boards = new List<BoardDesc>()
                }
            };
            Workspaces.Add(newwk);
            newwk.Init();
            Save();
            return newwk;
        }

        public void Delete(string workid)
        {
            var index = Workspaces.FindIndex(w => w.desc.id == workid);
            if (index != -1) Workspaces.RemoveAt(index);
            Save();
        }

        public void Dispose()
        {
            DB.Dispose();
        }

        public Workspace Get(string workid)
        {
            return Workspaces.Find(w => w.desc.id == workid);
        }

        public void Recovery()
        {
            var db = DB;
            var col = db.GetCollection<WorkspaceSaveData>("workspace");
            col.EnsureIndex(d => d.name);
            var old = col.FindOne(w => w.name == "main");
            if (old != null)
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
            if (old == null)
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
            foreach (var b in desc.boards)
            {
                // 根据类型创建对应模块实例
                var m = ModuleManager.Ins.BuildFrom(b.cardType);
                if (string.IsNullOrEmpty(b.id))
                {
                    b.id = Guid.NewGuid().ToString();
                }
                m.Workspace = this;
                m.ID = b.id;

                modules.Add(m);
                m.BoardDesc = b;
                m.Recovery();
                m.OnLoad();
            }
        }

        public void CreateBoard(string type)
        {
            try
            {
                var m = ModuleManager.Ins.BuildFrom(type);
                m.ID = Guid.NewGuid().ToString();
                modules.Add(m);
                m.Workspace = this;
                var b = new BoardDesc()
                {
                    cardType = type,
                    id = m.ID,
                    cName = m.GetType().GetCustomAttribute<PModuleAttribute>()?.ModuleName ?? "未命名"
                };
                desc.boards.Add(b);
                m.BoardDesc = b;
                m.OnFirstCreate();
                m.OnLoad();
                m.Save();
                ModuleManager.Ins.WorkspaceManager.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public void DeleteBoard(string id)
        {
            var dm = modules.Where(b => b.ID == id);
            foreach (var item in dm)
            {
                item.DeleteData();
                item.OnDestroy();
            }
            modules.RemoveAll(b => b.ID == id);
            desc.boards.RemoveAll(b => b.id == id);
            ModuleManager.Ins.WorkspaceManager.Save();
        }

        public void RenameBoard(string id, string newname)
        {
            desc.boards.Find(d => d.id == id)!.cName = newname;
            ModuleManager.Ins.WorkspaceManager.Save();
        }
        public void SetBoardGroup(string id, string newname)
        {
            desc.boards.Find(d => d.id == id)!.group = newname;
            ModuleManager.Ins.WorkspaceManager.Save();
        }
        public ModuleBase? TryFindModule(string moduleType)
        {
            return modules.Find(m => m.Type == moduleType);
        }

        public void SortBoards(List<string> newids)
        {
            var newarr = new List<BoardDesc>();
            foreach (var id in newids)
            {
                newarr.Add(desc.boards.Find(b => b.id == id)!);
            }
            desc.boards = newarr;
            ModuleManager.Ins.WorkspaceManager.Save();
        }

        public void Rename(string newname)
        {
            desc.wName = newname;
            ModuleManager.Ins.WorkspaceManager.Save();
        }
    }
}