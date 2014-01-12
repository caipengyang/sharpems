using System;
using System.Net.Sockets;
using System.Net;
using SharpChannel.Core;

namespace SharpChannel.Client
{
	public class TCPClient
	{
		private ClientSession session;
		public event NewPacketHandler NewPacketReceived;

		public TCPClient (string ip ,int port)
		{
			Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
			socket.Connect(new System.Net.IPEndPoint(IPAddress.Parse(ip),port));
			session = new ClientSession(socket);
		}

		public void SendMessage (string message)
		{
			byte[] buffer = System.Text.ASCIIEncoding.Default.GetBytes (message);
			session.Send (buffer);
			buffer = session.Receive (new byte[]{(byte)'$',(byte)'$',(byte)'$',(byte)'$'}, 5000);
			if (buffer != null) {
				Console.WriteLine ("client receive response message:" + System.Text.ASCIIEncoding.Default.GetString (buffer));
				if(NewPacketReceived != null) NewPacketReceived(this,new NewPacketArgs(buffer,session));
			} else {
				Console.WriteLine("nothing received.");
			}
		}
	}
}

