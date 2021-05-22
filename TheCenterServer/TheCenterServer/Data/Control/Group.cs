using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheCenterServer
{
    public class Group : UIControl
    {
        public Group() : base("group")
        {

        }

        [UIParam("子组件。")]
        public Group children(List<UICom> childs)
        {
            UI!.Prop["children"] = JsonSerializer.Serialize(childs, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return this;
        }
    }
}
