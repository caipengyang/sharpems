using System;

namespace TestRC
{
	public class ServerService : IService
	{
		public ServerService ()
		{
		}		

		#region IService implementation


		public string SayHello (string name)
		{
			return "你好:" + name;
		}


		#endregion



	}
}

