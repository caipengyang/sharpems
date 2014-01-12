using System;
using SharpChannel.Client;
using System.Text;

namespace SharpChannel.Server
{
	public class TestServer
	{
		public TestServer ()
		{
		}

		public static void NewClient (object obj)
		{
			TCPClient client = obj as TCPClient;
			if (client != null) {
				int times = new Random().Next() % 10000;
				for(int i = 0; i < times ; ++i)
				{
					StringBuilder sb = new StringBuilder();
					int length = new Random().Next() % 100;
					for(int p = 0; p < length ; ++p)
					{
						sb.Append((char)( p % 26 + 'a'));
					}
					client.SendMessage("message : " + sb.ToString() + "$$$$");
					System.Threading.Thread.Sleep(10000);
				}
			}
		}

		public static void Main (string[] args)
		{
			Console.WriteLine ("Test server start...");
			SharpChannel.Server.TCPServer server = new SharpChannel.Server.TCPServer ();
			server.NewPacketReceived += delegate(object sender, NewPacketArgs e) {
				string message = System.Text.ASCIIEncoding.Default.GetString (e.Data);
				Console.WriteLine ("-------------------------server received message  : " + message);
				//Console.WriteLine("send message back to client : " + message);
				e.Session.Send(System.Text.ASCIIEncoding.Default.GetBytes(message + "$$$$"));
			}; 
			server.Start ();
			System.Threading.Thread.Sleep (5000);
			TCPClient []clients = new TCPClient[10];
			for (int i = 0; i < 1; ++i) {
				clients[i] = new TCPClient("127.0.0.1",10086);
				System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(NewClient));
				thread.Start(clients[i]);
			}
			while (true) {
				if(Console.ReadLine().ToUpper().Equals("EXIT"))
				{
					Console.WriteLine("the server is going to stop...");
					break;
				}
			}
			server.Stop();
			Console.WriteLine("server stopped.");
		}
	}
}

