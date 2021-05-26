using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCenterServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PluginModuleController : ControllerBase
    {

        [HttpPost]
        public string RegisterPlugin(PluginRegisterInfo info)
        {
            return "HELLO";
        }

        public class PluginRegisterInfo
        {
            public string type { get; set; }

        }
    }
}
