using System;
using System.Collections.Generic;

namespace TheCenterServer.PModule
{
    [PModule("runscript")]
    public class RunScript : ModuleBase
    {
        [UI("runBtn")]
        ButtonControl runButton = new(Event: new List<EventBind>() { new("onclick", "run") });

        public RunScript()
        {
        }


        [Method("run")]
        string Run(string content)
        {
            return "RUN!" + content;
        }

        public override List<UICom> BuildInterface()
        {
            return new List<UICom>()
            {
                runButton.UI
            };
        }
    }
}