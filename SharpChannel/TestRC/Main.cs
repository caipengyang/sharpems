using System;

namespace TestRC
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Polenter.Serialization.SharpSerializer serializer = new Polenter.Serialization.SharpSerializer();
			Console.WriteLine ("Hello World!");
			TestRCServer server = new TestRCServer ();
			server.Start ();
			System.Threading.Thread.Sleep(5000);

			SharpChannel.Client.TCPClient client = new SharpChannel.Client.TCPClient("127.0.0.1",10086);
			while (true) {
				string cmd = Console.ReadLine().ToUpper();
				if(cmd == "EXIT")
				{
					server.Stop();
					break;
				}else{
					System.IO.MemoryStream stream = new System.IO.MemoryStream(102400);
					serializer.Serialize(cmd,stream);
					 byte[] bytes = new byte[stream.Length];
					stream.Seek(0,System.IO.SeekOrigin.Begin);
    				stream.Read(bytes, 0, bytes.Length); 
					string content = System.Text.ASCIIEncoding.Default.GetString(bytes);
					client.SendMessage("SayHello@$@" + content + "$$$$");
				}
			}
			Console.WriteLine("exit.");
		}
	}
}
