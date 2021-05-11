using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheCenterServer.PModule
{
    [PModule("runscript", "运行脚本")]
    public class RunScript : ModuleBase
    {

        [UI]
        Input scriptPathUI = new("", placeholder: "脚本路径".UTF8(), onChange: "OnChange");

        [UI]
        Button runBtn = new("运行".UTF8(), onClick: "Run");

        [UI]
        Text resText = new("", textAlign: TextAlign.left);

        [Persistence]
        public string result { get; set; } = "";

        [Persistence]
        public string scriptPath { get; set; } = "";

        public override Task OnLoad()
        {
            SetState(() =>
            {
                result = "";
            });
            return Task.CompletedTask;
        }

        [Method]
        string Run()
        {
            SetState(() =>
            {
                result = "";
                SyncUI();
            });
            var content = scriptPath;
            content = content.TrimStart('"');
            content = content.TrimEnd('"');
            if (!Directory.Exists(content) && !File.Exists(content))
            {
                SetState(() =>
                {
                    result = "该路径不存在！";
                    SyncUI();
                });
                return "";
            }

            Process pro = new Process();
            pro.StartInfo.FileName = content;
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.CreateNoWindow = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                SetState(() =>
                {
                    result += e.Data + "\n";
                    try
                    {
                        SyncUI();
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                    }

                });
            };
            try
            {
                pro.Start();
                pro.BeginOutputReadLine();
                pro.WaitForExitAsync();
            }
            catch (Exception e)
            {
                SetState(() =>
                {
                    result = e.ToString();
                    SyncUI();
                });
            }


            return "";
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
            if (result != "") ui.Add(resText.text(result));
            return ui;
        }
    }
}