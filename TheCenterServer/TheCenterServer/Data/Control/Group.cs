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
            UI!.prop["children"] = JsonSerializer.Serialize(childs, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return this;
        }

        [UIParam("水平布局。")]
        public Group Horizon(bool hor)
        {
            UI!.prop["hor"] = hor ? "true" : "false";
            return this;
        }

        [UIParam("间距。")]
        public Group Spacing(float hor_value, float? ver_value = null)
        {
            var s = new List<float> { hor_value };
            if(ver_value.HasValue)
            {
                s.Add(ver_value.Value);
            }
            UI!.prop["spacing"] = JsonSerializer.Serialize(s);
            return this;
        }
    }
}
