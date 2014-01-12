using System;
using System.Net.Sockets;
using SharpChannel.Core;

namespace SharpChannel
{
	public class NewPacketArgs : EventArgs
	{
		public byte[] Data
		{
			get;set;
		}

		public BaseSession Session
		{
			get;set;
		}

		public NewPacketArgs (byte[] data,BaseSession session)
		{
			this.Data = data;
			this.Session = session;
		}
	}

	public delegate void NewPacketHandler(object sender,NewPacketArgs args);

}
