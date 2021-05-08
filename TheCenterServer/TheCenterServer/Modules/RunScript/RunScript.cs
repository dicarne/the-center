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
        public RunScript()
        {
        }


        [Method("run")]
        string Run(string content)
        {
            WorkspaceHub.Ins.SendToClient(Workspace.ConnectID, Workspace.desc.Id, ID, JsonSerializer.Serialize(new List<UICom>()
            {
                runButton.UI,
                resText.UI
            }, new JsonSerializerOptions() { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
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