using System;
using System.Collections.Generic;

namespace TheCenterServer
{
    public class Workspace
    {
        public string WName { get; set; }
        public List<Board> Boards { get; set; }
    }

    public class Board
    {
        public string CardType { get; set; }
        public string CName { get; set; }
        public string Id { get; set; }
        public Dictionary<string, string> Prop { get; set; }
        public int W { get; set; } = 12;
        public int H { get; set; } = 1;
    }

    public class UICom
    {
        public string Id { get; set; }
        public string type { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public Dictionary<string, string> Prop { get; set; }
        public List<string> Event { get; set; }
    }

}