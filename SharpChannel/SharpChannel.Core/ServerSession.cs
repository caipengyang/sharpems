using System;
using System.Net;
using System.Net.Sockets;

namespace SharpChannel.Core
{
	public class ServerSession : BaseSession
	{
		public ServerSession (Socket socket) : base (socket)
		{
		}

		public new IPEndPoint EndPoint{
			get;
			set;
		}
	}
}

