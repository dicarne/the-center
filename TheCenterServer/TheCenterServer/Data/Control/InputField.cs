using System.Collections.Generic;


namespace TheCenterServer
{
    public class InputField : UIControl
    {
        public InputField(string text, string? onChange = null) : base()
        {
            UI = new UICom()
            {
                type = "inputfield"
            };
            UI.Prop.Add("text", text);

            var events = new List<EventBind>();
            if (onChange != null) events.Add(new("onChange", onChange));
            EventBind = events;
        }

        public InputField text(string? newtext)
        {
            UI!.Prop["text"] = newtext;
            return this;
        }
    }
}
