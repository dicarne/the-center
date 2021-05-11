﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TheCenterServer.PModule
{
    [PModule("opendirfile", "打开文件夹")]
    public class OpenDirFile : ModuleBase
    {
        [UI]
        Input scriptPathUI = new("", placeholder: "文件夹路径".UTF8(), onChange: "OnChange");

        [UI]
        Button runBtn = new("打开".UTF8(), onClick: "Run");

        [UI]
        Text resText = new("", textAlign: TextAlign.left);

        [Persistence]
        public string scriptPath { get; set; } = "";
        public string result { get; set; } = "";

        [Method]
        string Run(string content)
        {
            SetState(() =>
            {
                result = "";
                SyncUI();
            });
            Process pro = new Process();
            pro.StartInfo.FileName = "explorer";
            pro.StartInfo.Arguments = scriptPath;
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.CreateNoWindow = true;
            pro.StartInfo.RedirectStandardOutput = true;

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
