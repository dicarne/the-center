using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheCenterServer.PModule
{
    public class PluginModule : ModuleBase
    {
        public override Task OnLoad()
        {
            Run();
            return base.OnLoad();
        }

        static Dictionary<string, ChildModule> childrens = new();

        void Run()
        {
            Process pro = new Process();
            pro.StartInfo.FileName = config.interpreter;
            pro.StartInfo.Arguments = config.main;
            pro.StartInfo.WorkingDirectory = config.dir;
            pro.StartInfo.UseShellExecute = true;
            pro.StartInfo.CreateNoWindow = true;
            if (config.singleton)
            {
                childrens.Add(config.type, new ChildModule()
                {
                    process = pro,
                    config = config
                });
                var c = childrens[config.type];
                if (c.count == 0)
                {
                    try
                    {
                        pro.Start();
                        pro.Exited += (o, e) =>
                        {
                            var c = childrens[config.type];
                            c.count--;
                            if (c.count == 0)
                            {
                                childrens.Remove(config.type);
                            }
                        };
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
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

        }

        class ChildModule
        {
            public Process process { get; set; }
            public int count { get; set; }
            public ModuleManager.PluginModuleConfig config { get; set; }
        }
        string url => $"http://localhost:{config.port}/interface?work={Workspace.desc.Id}&board={BoardDesc.Id}&ui={ID}";

        public async override Task<List<UICom>> BuildInterface()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                Console.WriteLine(content);
                return JsonSerializer.Deserialize<List<UICom>>(content, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<UICom>() { new Text("ERROR PLUGIN: " + config.name) };
            }
        }

        public override object? HandleUIEvent(string control, string eventname, string[]? args = null)
        {
            return base.HandleUIEvent(control, eventname, args);
        }

        public override void OnCustomEvent(string control, string eventName, string[]? args = null)
        {
            base.OnCustomEvent(control, eventName, args);
        }
    }
}
