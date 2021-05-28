using System;
using System.Collections.Generic;

namespace TheCenterServer
{
    public class WorkspaceDesc
    {
        public string wName { get; set; }
        public string id { get; set; }
        public List<BoardDesc> boards { get; set; } = new();
        public List<GroupBoardColor> groups { get; set; } = new();
        public Dictionary<string, string> globalVariables { get; set; } = new();
    }

    public class GroupBoardColor
    {
        public string name { get; set; }
        public string color { get; set; }
    }

    /// <summary>
    /// 卡片的描述信息
    /// </summary>
    public class BoardDesc
    {
        /// <summary>
        /// 卡片类型，通过此字符串实例化UI控件
        /// </summary>
        public string cardType { get; set; }
        public string cName { get; set; }
        public string group { get; set; }
        public string id { get; set; }
        public Dictionary<string, string> prop { get; set; } = new();
        public int w { get; set; } = 6;
        public int h { get; set; } = 1;
        public bool hide { get; set; } = false;
    }

    public class BoardUI : BoardDesc
    {
        public List<UICom> uIComs { get; set; } = new();
        public static BoardUI From(BoardDesc desc, List<UICom> uis)
        {
            var bd = new BoardUI()
            {
                cardType = desc.cardType,
                cName = desc.cName,
                h = desc.h,
                id = desc.id,
                prop = desc.prop,
                w = desc.w,
                uIComs = uis,
                group = desc.group
            };
            return bd;
        }
    }

    public class UICom
    {
        public string id { get; set; }
        public string type { get; set; }
        public Dictionary<string, string?> style { get; set; } = new();
        public Dictionary<string, string?> prop { get; set; } = new();
        public List<string> eventlist { get; set; } = new();
        public UICom() { }
        public UICom(string type) { this.type = type; }
    }

}