using System;
using System.Collections.Generic;

namespace TheCenterServer.Module
{
    public class RunScript : ModuleBase
    {
        [UI("runBtn")]
        ButtonControl runButton = new(Event: new List<EventBind>() { new EventBind("onclick", "run") })
        {
            W = 10,
            H = 30
        };


        [Method("run")]
        void Run()
        {

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