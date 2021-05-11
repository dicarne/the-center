using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCenterServer.PModule
{
    [PModule("runmany", "运行其他卡片")]
    public class RunMany: ModuleBase
    {
        [UI]
        public Button runBtn = new Button("运行".UTF8(), "Run");

        [Method]
        public void Run()
        {
            foreach (var item in Workspace.modules)
            {
                try
                {
                    item.Excute();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            
        }

        public override List<UICom> BuildInterface()
        {
            return new()
            {
                runBtn
            };
        }
    }
}
