using System;
using System.Collections.Generic;
using System.Text.Json;

namespace TheCenterServer.PModule
{
    [PModule("runscript")]
    public class RunScript : ModuleBase
    {

        [UI]
        InputField scriptPathUI = new("", placeholder: "½Å±¾Â·¾¶".UTF8(), onChange: "OnChange");

        [UI]
        Button runBtn = new(onClick: "Run");

        [UI]
        Text resText = new("YES!");

        [Persistence]
        public bool result { get; set; } = false;

        [Persistence]
        public string scriptPath { get; set; } = "";

        [Method]
        string Run(string content)
        {
            SetState(() =>
            {
                result = true;

            });
            WorkspaceHub.Ins.SendUIToClient(Workspace.ConnectID, Workspace.desc.Id, ID, BuildInterface());
            return "RUN!" + content;
        }

        [Method]
        void OnChange(string newcontent)
        {
            SetState(() =>
            {
                scriptPath = newcontent;
            });
        }

        public override List<UICom> BuildInterface()
        {
            var ui = new List<UICom>()
            {
                scriptPathUI.text(scriptPath),
                runBtn
            };
            if (result) ui.Add(resText);
            return ui;
        }
    }
}