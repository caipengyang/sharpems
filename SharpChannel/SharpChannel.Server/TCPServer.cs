using System;
using System.Net.Sockets;
using System.Net;
using SharpChannel.Core;

namespace SharpChannel.Server
{
	public class TCPServer
	{
		public TCPServer ()
		{
			clients = new System.Collections.Generic.List<Socket> ();
			client2Sessions = System.Collections.Hashtable.Synchronized (new System.Collections.Hashtable ());
		}

		public void Start (int port = 10086)
		{
			serverSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			serverSocket.Bind (new IPEndPoint (IPAddress.Any, port));
			acceptThread = new System.Threading.Thread (
								new System.Threading.ThreadStart (RunServer));
			packetThread = new System.Threading.Thread (
									new System.Threading.ThreadStart (RunPacket));
			acceptThread.Start (serverSocket);
			packetThread.Start ();
		}

		private void RunServer ()
		{
			serverSocket.Listen(10);
			status = ServerStatus.running;
			while (status == ServerStatus.running) {
				if (serverSocket.Poll (1000000, SelectMode.SelectRead)) {
					this.AcceptNewClient (serverSocket.Accept ());
				} else {
					System.Threading.Thread.Sleep (1000);
				}
			}
		}

		private void RunPacket ()
		{
			status = ServerStatus.running;
			System.Collections.Generic.List<Socket> readSockets = new System.Collections.Generic.List<Socket> ();
			System.Collections.Generic.List<Socket> errorSockets = new System.Collections.Generic.List<Socket> ();
			while (status == ServerStatus.running) {
				if(clients.Count <= 0)
				{
					System.Threading.Thread.Sleep(1000);
					continue;
				}
				readSockets.AddRange (clients);
				errorSockets.AddRange(clients);
				Socket.Select (readSockets, null, errorSockets, 100);
				foreach (Socket sock in errorSockets) {
					clients.Remove (sock);
					client2Sessions.Remove (sock);
				}
				foreach (Socket sock in readSockets) {
					AccpetMessage(sock);
				}
				readSockets.Clear ();
				errorSockets.Clear ();
			}
		}

		private void AcceptNewClient (Socket client)
		{
			clients.Add (client);
			client2Sessions.Add (client, new SharpChannel.Core.ServerSession (client));
		}

		private void AccpetMessage (Socket client)
		{
			byte[] buffer = new byte[10240];
			int size = client.Receive (buffer);
			if (size > 0) {
				ServerSession session = client2Sessions [client] as ServerSession;
				if (session != null) {
					session.Receive (buffer, 0, size);
					byte[] packet = session.Analise(new byte[]{(byte)'$',(byte)'$',(byte)'$',(byte)'$'});
					if(packet != null && NewPacketReceived != null)
					{
						NewPacketReceived(this,new NewPacketArgs(packet,session));
					}
				}
			}
		}

		public void Stop ()
		{
			status = ServerStatus.stoping;
			acceptThread.Join ();
			status = ServerStatus.closed;
		}

		public ServerStatus Status ()
		{
			return status;
		}

		private Socket serverSocket;
		private System.Collections.Generic.IList<Socket> clients;
		private System.Collections.Hashtable client2Sessions ;
		private System.Threading.Thread acceptThread, packetThread;
		private ServerStatus status = ServerStatus.none;
		public event NewPacketHandler NewPacketReceived;
	}

	public enum ServerStatus
	{
		none,
		running,
		stoping,
		closed
	}
}

