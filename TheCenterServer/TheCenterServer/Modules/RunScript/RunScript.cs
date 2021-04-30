using System;
using System.Collections.Generic;

namespace TheCenterServer.PModule
{
	public class RunScript : ModuleBase
	{
		[UI("runBtn")]
		ButtonControl runButton = new ButtonControl(Event: new List<EventBind>() { new EventBind("onclick", "run") });
		public RunScript()
		{
			runButton.UI.W = 10;
			runButton.UI.H = 30;
		}


		[Method("run")]
		string Run()
		{
			Console.WriteLine("RUN!");
			return "RUN!";
		}

		public override List<UICom> BuildInterface()
		{
			return new List<UICom>()
			{
				runButton.UI
			};
		}
	}
}