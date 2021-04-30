using System;
using Xunit;
using TheCenterServer.PModule;
using System.Threading.Tasks;

namespace Test
{
	public class UnitTest1
	{
		public UnitTest1() { 
		
		}

		[Fact]
		public void Test1()
		{
			var m = new RunScript();
			var ret = m.HandleEvent("runBtn", "onclick");
			Assert.Equal("RUN!", ret as string);
		}
	}
}
