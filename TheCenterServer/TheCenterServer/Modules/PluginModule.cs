﻿using Microsoft.AspNetCore.Http;
using StackExchange.Profiling;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheCenterServer.PModule
{
    public class PluginModule : ModuleBase
    {
        [Persistence]
        public Dictionary<string, string> Data { get; set; } = new();
        public override Task OnLoad()
        {
            Run();
            return base.OnLoad();
        }

        static Dictionary<string, ChildModule> childrens = new();
        Process process;
        void Run()
        {
            Process pro = new Process();
            process = pro;
            pro.StartInfo.FileName = config.interpreter;
            pro.StartInfo.Arguments = config.main;
            pro.StartInfo.WorkingDirectory = config.dir;
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.CreateNoWindow = false;

            pro.StartInfo.RedirectStandardOutput = true;
            //pro.StartInfo.RedirectStandardError = true;
            pro.OutputDataReceived += (o, e) =>
            {
                File.AppendAllTextAsync(Path.Combine(config.dir, "log.log"), (e.Data ?? "") + "\n");
                if(e.Data.HasValue() && !e.Data!.StartsWith("INFO:"))
                {
                    Console.WriteLine(e.Data + "\n");
                }
                
            };
            /*pro.ErrorDataReceived += (o, e) =>
            {
                File.AppendAllTextAsync(Path.Combine(config.dir, "error.log"), (e.Data ?? "") + "\n");
                if (e.Data.HasValue() && !e.Data!.StartsWith("INFO:"))
                {
                    Console.WriteLine(e.Data + "\n");
                }
            };*/

            if (config.singleton)
            {
                if (childrens.ContainsKey(config.type))
                {

                }
                else
                {
                    childrens.Add(config.type, new ChildModule()
                    {
                        process = pro,
                        config = config
                    });
                }

                var c = childrens[config.type];
                if (c.count == 0)
                {
                    try
                    {
                        pro.Start();
                        pro.BeginOutputReadLine();
                        pro.Exited += (o, e) =>
                        {
                            Console.WriteLine(e.ToJson());
                        };
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    process = childrens[config.type].process;
                }
                c.count++;
            }
            else
            {
                try
                {
                    pro.Start();


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            InitModule();

        }

        bool init = false;
        Task<HttpResponseMessage> initTask;
        async Task InitModule()
        {
            if (init) return;
            await client.PostAsJsonAsync(url + $"/init?work={Workspace.desc.id}&board={BoardDesc.id}", BoardDesc);
            init = true;
        }

        class ChildModule
        {
            public Process process { get; set; }
            public int count { get; set; }
            public ModuleManager.PluginModuleConfig config { get; set; }
        }
        string url => $"http://127.0.0.1:{config.port}";

        static HttpClient client = new HttpClient();
        public async override Task<List<UICom>> BuildInterface()
        {
            var response = await client.GetAsync(url + $"/interface?work={Workspace.desc.id}&board={BoardDesc.id}");
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                var r = JsonSerializer.Deserialize<List<UICom>>(content, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })!;
                return r;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<UICom>() { new Text("ERROR PLUGIN: " + config.name) };
            }
        }

        class UIEventReq
        {
            public string control { get; set; }
            public string eventname { get; set; }
            public string[]? args { get; set; }
        }
        public override object? HandleUIEvent(string control, string eventname, string[]? args = null)
        {
            var response = client.PostAsJsonAsync(url + $"/uievent?work={Workspace.desc.id}&board={BoardDesc.id}",
                new UIEventReq
                {
                    control = control,
                    eventname = eventname,
                    args = args
                }
            );
            OnGlobalVariableChange();
            return null;
        }

        public override void OnGlobalVariableChange()
        {
            client.PostAsJsonAsync(url + $"/globalvariable?work={Workspace.desc.id}",
                Workspace.desc.globalVariables
            );
        }
    }
}
