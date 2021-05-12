using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheCenterServer.PModule
{
    [PModule("runmany", "运行其他卡片")]
    public class RunMany : ModuleBase
    {
        [UI]
        public Button runBtn = new Button("运行".UTF8(), "Run");

        [UI]
        public Transfer runList = new(new(), new(), text: "选择卡片".UTF8(), type: "local_boards", onChange: "onChange", onShow: "onShow");

        [Persistence]
        public List<string> available { get; set; } = new();

        [Persistence]
        public List<string> unavailable { get; set; } = new();

        [Method]
        public void Run()
        {
            var ava = new HashSet<string>(available);
            foreach (var item in Workspace.modules)
            {
                try
                {
                    if (ava.Contains(item.ID))
                    {
                        item.Excute();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

        }

        /// <summary>
        /// 保存列表更改
        /// </summary>
        [Method]
        public void onChange(string ava, string unava)
        {
            SetState(() =>
            {
                available = JsonSerializer.Deserialize<List<string>>(ava)!;
            });
        }

        [Method]
        public Transfer.ShowData onShow()
        {
            unavailable.Clear();
            var all = Workspace.desc.Boards.Select(b => b.Id);
            return new Transfer.ShowData(available, all.ToList());
        }

        public override List<UICom> BuildInterface()
        {
            return new()
            {
                runList,
                runBtn
            };
        }
    }
}
