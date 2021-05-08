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
		public ButtonControl(List<EventBind> Event) : base(Event)
		{
			UI = new Button()
			{
				type = "button",
				Event = Event.Select(e => e.eventname).ToList()
			};
		}
	}

	public class Text : UICom
	{
		
	}

	public class TextControl : UIControl
	{
		public TextControl(string text) : base(new())
		{
			UI = new Text()
			{
				type = "text",
				Event = new(),
			};
			UI.Prop.Add("text", text);
		}
	}
}