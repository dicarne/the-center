using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TheCenterServer.PModule
{
    [PModule("runprogram", "运行程序")]
    public class RunProgram : ModuleBase
    {
        [UI]
        Input commandUI = new("", placeholder: "命令与参数".UTF8(), onChange: "CommandChange");
        [UI]
        Input scriptPathUI = new("", placeholder: "文件夹路径".UTF8(), onChange: "PathChange");

        [UI]
        Button runBtn = new("打开".UTF8(), onClick: "Excute");

        [UI]
        Text resText = new("", textAlign: TextAlign.left);
        public string result { get; set; } = "";

        [Persistence]
        public string scriptPath { get; set; } = "";
        [Persistence]
        public string command { get; set; } = "";

        [Method]
        public override void Excute()
        {
            SetState(() =>
            {
                result = "";
                SyncUI();
            });
            var dir = scriptPath;
            dir = dir.TrimStart('"');
            dir = dir.TrimEnd('"');
            if (!Directory.Exists(dir) && !File.Exists(dir))
            {
                SetState(() =>
                {
                    result = "该路径不存在！";
                    SyncUI();
                });
                return;
            }
            dir = Path.GetDirectoryName(dir);
            var s = command.Split(" ");
            var program = s[0];
            Process pro = new Process();
            pro.StartInfo.FileName = program;
            pro.StartInfo.Arguments = command.Substring(program.Length);
            pro.StartInfo.WorkingDirectory = dir;
            pro.StartInfo.UseShellExecute = true;
            pro.StartInfo.CreateNoWindow = true;

            try
            {
                pro.Start();
            }
            catch (Exception e)
            {
                SetState(() =>
                {
                    result = e.ToString();
                    SyncUI();
                });
            }
        }

        [Method]
        void CommandChange(string newstr)
        {
            SetState(() =>
            {
                command = newstr;
            });
        }

        [Method]
        void PathChange(string newstr)
        {
            SetState(() =>
            {
                scriptPath = newstr;
            });
        }

        public override List<UICom> BuildInterface()
        {
            var ui = new List<UICom>() {
                commandUI.text(command),
                scriptPathUI.text(scriptPath),
                runBtn
            };
            if (result != "") ui.Add(resText.text(result));
            return ui;
        }
    }
}
