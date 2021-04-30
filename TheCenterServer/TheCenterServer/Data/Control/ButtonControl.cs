using System;
using System.Collections.Generic;
using System.Linq;

namespace TheCenterServer
{
    public class Button : UICom
    {
    }

    public class ButtonControl : UIControl
    {
        public ButtonControl(List<EventBind> Event = null) : base(Event)
        {
            UI = new Button()
            {
                Event = Event.Select(e => e.eventname).ToList() ?? new List<string>()
            };
        }
    }
}