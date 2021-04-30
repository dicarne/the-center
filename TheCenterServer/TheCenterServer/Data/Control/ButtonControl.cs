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
}