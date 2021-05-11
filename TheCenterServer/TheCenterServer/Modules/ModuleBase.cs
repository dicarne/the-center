using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LiteDB;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TheCenterServer.PModule
{
    public class ModuleBase
    {
        public Workspace Workspace { get; set; }
        public BoardDesc BoardDesc { get; set; }
        public string ID { get; set; }
        public string Type { get; private set; }
        /// <summary>
        /// 注册控件
        /// </summary>
        Dictionary<string, UIControl> controls = new();

        /// <summary>
        /// 注册函数
        /// </summary>
        Dictionary<string, MethodInfo> methods = new();

        Dictionary<string, PropertyInfo> needPersist = new();
        public ModuleBase()
        {
            var type = this.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Instance;
            var typeattr = type.GetCustomAttribute<PModuleAttribute>(false);
            if (typeattr == null)
            {
                Console.WriteLine("[ERR] 类型名称不能为空。");
            }
            else
            {
                Type = typeattr.ModuleType;
            }

            var UIs = type.GetFields(flags).Where(p => p.GetValue(this).GetType().IsSubclassOf(typeof(UIControl)));
            foreach (var ui in UIs)
            {
                var attr = ui.GetCustomAttributes(typeof(UIAttribute), true).FirstOrDefault();
                if (attr != null && attr is UIAttribute ua)
                {
                    if (ui.GetValue(this) is UIControl ins)
                    {
                        ins.Id = ua.name ?? ui.Name;
                        controls.Add(ins.Id, ins);
                    }
                }
            }
            var funs = type.GetMethods(flags).Where(p => p.GetCustomAttributes(typeof(MethodAttribute), true).FirstOrDefault() != null);
            foreach (var item in funs)
            {
                var a = item.GetCustomAttributes(typeof(MethodAttribute), true).FirstOrDefault();
                if (a is MethodAttribute ma)
                {
                    methods.Add(ma.name ?? item.Name, item);
                }
            }
            var persists = type.GetProperties(flags).Where(p => p.GetCustomAttribute(typeof(PersistenceAttribute), true) != null);
            foreach (var p in persists)
            {
                needPersist.Add(p.Name, p);
            }
        }

        public virtual void OnFirstCreate()
        {

        }

        public virtual Task OnLoad()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnDestroy()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 提供给前端UI描述信息。
        /// </summary>
        /// <returns></returns>
        public virtual List<UICom> BuildInterface()
        {
            return new List<UICom>();
        }


        /// <summary>
        /// 主运行入口，通常供其他卡片调用
        /// </summary>
        public virtual void Excute()
        {

        }

        /// <summary>
        /// 分发控件事件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventname"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object? HandleUIEvent(string control, string eventname, string[]? args = null)
        {
            if (controls.TryGetValue(control, out var ins))
            {
                var eventbind = ins.EventBind.FirstOrDefault(e => e.eventname == eventname);
                if (eventbind != null)
                {
                    var method = eventbind.method;
                    if (methods.TryGetValue(method, out var minfo))
                    {
                        return minfo.Invoke(this, args);
                    }
                    else
                    {
                        Console.WriteLine($"[ERR] 找不到绑定方法{control}.{eventname}.{method}。");
                    }
                }
            }
            return null;
        }

        class ModuleSaveData
        {
            public ObjectId? _id { get; set; }
            public string? workspace { get; set; }
            public string? board { get; set; }
            public string? data { get; set; }
        }

        public void DeleteData()
        {
            var db = WorkspaceManager.DB;
            var col = db.GetCollection<ModuleSaveData>("ModuleData");
            col.EnsureIndex(d => d.board);
            var old = col.FindOne(c => c.board == ID);
            if(old != null)
            {
                col.Delete(old._id);
            }
        }

        public void Save()
        {
            var db = WorkspaceManager.DB;
            var col = db.GetCollection<ModuleSaveData>("ModuleData");
            col.EnsureIndex(d => d.board);
            var old = col.FindOne(c => c.board == ID);
            if (old != null)
            {
                old.data = GetSaveData();
                col.Update(old);
            }
            else
            {
                col.Insert(new ModuleSaveData()
                {
                    workspace = Workspace.desc.Id,
                    board = ID,
                    data = GetSaveData()
                });
            }
            Dirt = false;
        }
        bool Dirt { get; set; }
        protected void SyncUI()
        {
            WorkspaceBackgroundService.Ins.SendUIToClient(Workspace.ConnectID, Workspace.desc.Id, ID, BuildInterface());
        }
        protected void SetState(Action action)
        {
            action();
            Dirt = true;
            Save();
        }
        string GetSaveData()
        {
            var dic = new Dictionary<string, string>();
            foreach (var p in needPersist)
            {
                dic.Add(p.Key, JsonSerializer.Serialize(p.Value.GetValue(this)));
            }
            return JsonSerializer.Serialize(dic);
        }

        public void Recovery()
        {
            var db = WorkspaceManager.DB;
            var col = db.GetCollection<ModuleSaveData>("ModuleData");
            var old = col.FindOne(c => c.workspace == Workspace.desc.Id && c.board == ID);
            if (old == null)
            {
                Console.WriteLine($"[WARN] 找不到{Workspace.desc.Id}.{ID}的记录。");
                return;
            }
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(old.data!);
            if (data == null)
            {
                return;
            }
            foreach (var item in data)
            {
                if (needPersist.TryGetValue(item.Key, out var ptype))
                {
                    try
                    {
                        var obj = JsonSerializer.Deserialize(item.Value, ptype.PropertyType);
                        ptype.SetValue(this, obj);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine($"[IGNORE] {item.Key}");
                    }

                }
                else
                {
                    Console.WriteLine($"[WARN] 找不到{Workspace.desc.Id}.{ID}.{item.Key}的记录。");
                }
            }
        }

    }

    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class PModuleAttribute : Attribute
    {

        public PModuleAttribute(string moduleType, string name)
        {
            ModuleType = moduleType;
            ModuleName = name;
        }
        public string ModuleType { get; set; }
        public string ModuleName { get; set; }
    }

    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class PersistenceAttribute : Attribute
    {
        public PersistenceAttribute()
        {

        }
    }
}