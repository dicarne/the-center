using System;
using System.Collections.Generic;

namespace TheCenterServer
{
    public class WorkspaceDesc
    {
        public string WName { get; set; }
        public string Id { get; set; }
        public List<BoardDesc> Boards { get; set; } = new();
        public List<GroupBoardColor> Groups { get; set; } = new();
    }

    public class GroupBoardColor
    {
        public string name { get; set; }
        public string color { get; set; }
    }

    /// <summary>
    /// ��Ƭ��������Ϣ
    /// </summary>
    public class BoardDesc
    {
        /// <summary>
        /// ��Ƭ���ͣ�ͨ�����ַ���ʵ����UI�ؼ�
        /// </summary>
        public string CardType { get; set; }
        public string CName { get; set; }
        public string Group { get; set; }
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
            var bd = new BoardUI()
            {
                CardType = desc.CardType,
                CName = desc.CName,
                H = desc.H,
                Id = desc.Id,
                Prop = desc.Prop,
                W = desc.W,
                uIComs = uis,
                Group = desc.Group
            };
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