using System;

namespace SharpChannel.Common
{
	/// <summary>
	/// 缓冲区队列，可以自增.
	/// </summary>
	public interface IBufferQueue <T> : IUnique
	{
		/// <summary>
		/// Ens the queue.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="offset">Offset.</param>
		/// <param name="size">Size.</param>
		void EnQueue(T[] array,int offset = 0,int size = -1);

		/// <summary>
		/// Des the queue.
		/// </summary>
		/// <returns>The queue.</returns>
		/// <param name="offset">Offset.</param>
		/// <param name="size">Size.</param>
		T[]  DeQueue(int offset = 0,int size = -1);

		/// <summary>
		/// Slice the specified offset and size.
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <param name="size">Size.</param>
		T[]  Slice (int offset = 0, int size = -1);
		
		/// <summary>
		/// Des the queue.
		/// </summary>
		/// <returns>The queue.</returns>
		/// <param name="feature">Feature.</param>
		T[] DeQueue(T[] feature);

		/// <summary>
		/// Des the queue.
		/// </summary>
		/// <returns>The queue.</returns>
		/// <param name="startFeature">Start feature.</param>
		/// <param name="endFeature">End feature.</param>
		T[] DeQueue(T[] startFeature,T[] endFeature);

		/// <summary>
		/// At the specified index.
		/// </summary>
		/// <param name='index'>
		/// Index.
		/// </param>
		T At(int index);
	}
}

