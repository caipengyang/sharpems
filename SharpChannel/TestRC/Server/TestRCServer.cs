using System;

namespace TestRC
{
	public class TestRCServer
	{
		private SharpChannel.Server.TCPServer server ;
		private IService service ;
		private Polenter.Serialization.SharpSerializer serializer = new Polenter.Serialization.SharpSerializer();
		public TestRCServer ()
		{
			service = new ServerService();
			server = new SharpChannel.Server.TCPServer();
		}

		public void Start ()
		{
			server.NewPacketReceived +=delegate(object sender, SharpChannel.NewPacketArgs args) {
				string message = System.Text.ASCIIEncoding.Default.GetString(args.Data);
				string []array = message.Split(new string[] {"@$@"},StringSplitOptions.RemoveEmptyEntries);
				object []param = new Object[array.Length -1];
				for(int i = 1 ; i < array.Length ; ++i)
				{
					param[i-1] = serializer.Deserialize(new System.IO.MemoryStream(System.Text.ASCIIEncoding.Default.GetBytes(array[i])));
				}
				object result = service.GetType().InvokeMember(array[0],System.Reflection.BindingFlags.Default| System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Instance,null,
				                               service,param);
				Console.WriteLine("server result :" + result);
			};
			server.Start();
		}

		public void Stop()
		{
			server.Stop();
		}
	}
}

