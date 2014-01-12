using System;
using System.Net.Sockets;

namespace SharpChannel.Core
{
	public abstract class BaseSession : ISession
	{
		#region ISession implementation

		public int Send (byte[] buffer, int offset = 0, int length = -1)
		{
			if (length < 0)
				length = buffer.Length;
			int sent = 0;
			while(sent < length) sent += socket.Send (buffer, offset + sent , length - sent,SocketFlags.None);
			return sent;
		}

		public void Receive (byte[] buffer, int offset = 0,int length = -1)
		{
			this.bufferQueue.EnQueue (buffer, offset,length);
		}

		public byte[] Analise (byte[] feature)
		{
			return this.bufferQueue.DeQueue(feature);
		}

		public void AsyncSend (byte[] buffer, AsyncCallback callBack = null, int offset = 0, int length = -1)
		{
			throw new NotImplementedException ();
		}

		public byte[] Receive (byte[] feature, int millseconds)
		{
			long curentTime = System.DateTime.Now.Ticks;
			byte[] result = null;
			byte[] buffer= new byte[10240];
			while ((result = this.Analise(feature)) == null) {
				if(socket.Poll(100000000,SelectMode.SelectRead))
				{
					int iLen = socket.Receive(buffer);
					if(iLen > 0)
					{
						this.Receive(buffer,0,iLen);
					}
				}
				else{
					System.Threading.Thread.Sleep(100);
				}
				if((System.DateTime.Now.Ticks - curentTime) / 10000 > millseconds) 
				{
					break;
				}
			}
			return result;
		}

	
		public System.Net.IPEndPoint EndPoint{get;set;}

		#endregion

		#region IUnique implementation

		public long ID {
			get ;
			set ;
		}

		#endregion

		public BaseSession (Socket socket)
		{
			this.socket = socket;
			this.ID = globalSequence++;
			this.bufferQueue = new SharpChannel.Common.RingBufferQueue<byte> ();
		}

		public readonly Socket socket;
		private SharpChannel.Common.IBufferQueue<byte> bufferQueue;
		protected static int globalSequence = 1;
	}
}

