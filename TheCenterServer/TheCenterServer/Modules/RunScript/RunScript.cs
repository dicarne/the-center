using System;
using System.Collections.Generic;
using System.Text.Json;

namespace TheCenterServer.PModule
{
    [PModule("runscript")]
    public class RunScript : ModuleBase
    {
        [UI("runBtn")]
        ButtonControl runButton = new(Event: new List<EventBind>() { new("onclick", "run") });

        [UI("resText")]
        TextControl resText = new("YES!");

        [Persistence]
        public bool result { get; set; } = false;

        [Method("run")]
        string Run(string content)
        {
            result = true;
            WorkspaceHub.Ins.SendUIToClient(Workspace.ConnectID, Workspace.desc.Id, ID, BuildInterface());
            SetDirt();
            return "RUN!" + content;
        }

        public override List<UICom> BuildInterface()
        {
            var ui = new List<UICom>()
            {
                runButton
            };
            if (result) ui.Add(resText);
            return ui;
        }
    }
}