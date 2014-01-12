using System;
using System.Net;

namespace SharpChannel.Core
{
	public interface ISession : SharpChannel.Common.IUnique
	{

		IPEndPoint EndPoint{get;set;}

		/// <summary>
		/// Send the specified buffer, offset and length.
		/// </summary>
		/// <param name="buffer">Buffer.</param>
		/// <param name="offset">Offset.</param>
		/// <param name="length">Length.</param>
		int Send(byte[] buffer,int offset = 0,int length = -1);

		/// <summary>
		/// Receive the specified buffer and offset.
		/// </summary>
		/// <param name="buffer">Buffer.</param>
		/// <param name="offset">Offset.</param>
		void Receive(byte[] buffer,int offset = 0,int length = -1 );

		/// <summary>
		/// Asyncs the send.
		/// </summary>
		/// <param name="buffer">Buffer.</param>
		/// <param name="callBack">Call back.</param>
		/// <param name="offset">Offset.</param>
		/// <param name="length">Length.</param>
		void AsyncSend(byte[] buffer,AsyncCallback callBack = null , int offset = 0,int length = -1);
	}
}

