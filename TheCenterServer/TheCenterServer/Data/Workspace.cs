using System;
using System.Collections.Generic;

namespace TheCenterServer
{
    public class WorkspaceDesc
    {
        public string WName { get; set; }
        public string Id { get; set; }
        public List<BoardDesc> Boards { get; set; }
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
        public string Id { get; set; }
        public Dictionary<string, string> Prop { get; set; }
        public int W { get; set; } = 12;
        public int H { get; set; } = 1;
    }

    public class BoardUI: BoardDesc
    {
        public List<UICom> uIComs { get; set; }
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
        public Dictionary<string, string> Style { get; set; }
        public Dictionary<string, string> Prop { get; set; }
        public List<string> Event { get; set; }
    }

}