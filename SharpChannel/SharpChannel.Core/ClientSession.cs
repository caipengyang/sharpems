using System;
using System.Net.Sockets;
using System.Net;

namespace SharpChannel.Core
{
	public class ClientSession : BaseSession
	{
		public ClientSession (Socket socket):base(socket)
		{
		}

		public new IPEndPoint EndPoint{
			get;
			set;
		}
	}
}

