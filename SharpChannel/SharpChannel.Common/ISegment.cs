using System;

namespace SharpChannel.Common
{
	public interface ISegment<T> : IUnique
	{
		/// <summary>
		/// Append the specified array, offset and length.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="offset">Offset.</param>
		/// <param name="length">Length.</param>
		void Append(T[] array,int offset = 0,int length = -1);

		/// <summary>
		/// Slice the specified offset and length.
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <param name="length">Length.</param>
		T[] Slice(int offset,int length);

		/// <summary>
		/// Remove the specified offset and length.
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <param name="length">Length.</param>
		T[] Remove(int offset,int length);

		/// <summary>
		/// At the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		T At(int index);
		
	}
}

