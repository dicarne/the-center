using System;
using System.Collections.Generic;

namespace TheCenterServer
{
    public class WorkspaceDesc
    {
        public string WName { get; set; }
        public string Id { get; set; }
        public List<BoardDesc> Boards { get; set; } = new();
    }

    /// <summary>
    /// 卡片的描述信息
    /// </summary>
    public class BoardDesc
    {
        /// <summary>
        /// 卡片类型，通过此字符串实例化UI控件
        /// </summary>
        public string CardType { get; set; }
        public string CName { get; set; }
        public string Id { get; set; }
        public Dictionary<string, string> Prop { get; set; } = new();
        public int W { get; set; } = 6;
        public int H { get; set; } = 1;
        public bool Hide { get; set; } = false;
    }

    public class BoardUI : BoardDesc
    {
        public List<UICom> uIComs { get; set; } = new();
        public static BoardUI From(BoardDesc desc, List<UICom> uis)
        {
            var bd = new BoardUI() { CardType = desc.CardType, CName = desc.CName, H = desc.H, Id = desc.Id, Prop = desc.Prop, W = desc.W, uIComs = uis };
            return bd;
        }
    }

    public class UICom
    {
        public string Id { get; set; }
        public string type { get; set; }
        public Dictionary<string, string?> Style { get; set; } = new();
        public Dictionary<string, string?> Prop { get; set; } = new();
        public List<string> Event { get; set; } = new();
        public UICom() { }
        public UICom(string type) { this.type = type; }
    }

}