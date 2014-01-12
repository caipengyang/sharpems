using System;

namespace SharpChannel.Common
{
	public sealed class RingBufferQueue<T> :  IBufferQueue<T> where T : IEquatable<T>
	{
		#region IBufferQueue implementation

		public void EnQueue (T[] array, int offset = 0, int size = -1)
		{
			if (size <= 0)
				size = array.Length - offset;
			if (size <= 0)
				return;
			while (RemainSize () < size)
				Expand ();
			if (head <= tail) {
				if (buffer.Length - tail >= size) {
					Array.Copy (array, offset, buffer, tail, size);
					tail += size;
				} else {
					Array.Copy (array, offset, buffer, tail, buffer.Length - tail);
					Array.Copy (array, offset + buffer.Length - tail, buffer, 0, size + tail - buffer.Length);
					tail = size + tail - buffer.Length;
				}
			} else {
				Array.Copy(array,offset,buffer,tail,size);
				tail += size;
			}

		}

		public T[] DeQueue (int offset = 0, int size = -1)
		{
			T[] result = Slice (offset, size);
			if (result != null) {
				head = (head + result.Length) % buffer.Length;
				return result;
			} else {
				return null;
			}
		}

		public T[] Slice(int offset = 0,int size = -1)
		{
			if (size < 0)
				size = this.Length - offset;
			size = Math.Min(size,this.Length);
			if (size <= 0)
				return null;
			T[] result = new T[size];
			int start = (head + offset) % buffer.Length;
			int end = (head + offset + size) % buffer.Length;
			if (end > start) {
				Array.Copy (buffer, start, result, 0, size);
				//head += size;
			} else {
				Array.Copy(buffer,start,result,0,buffer.Length - start);
				Array.Copy(buffer,0,result,buffer.Length - start,size + start - buffer.Length);
				//head = (head + size) % buffer.Length;
			}
			return result;
		}

		private int FirstOf (T[] feature, int start = -1)
		{
			int matched = 0;
			if(start <0 ) start = head;
			for (int i = start; i != tail; i = (i + 1) % buffer.Length) {
				if ((buffer [i]).Equals(feature [matched])) {
					if (matched++ == 0)
						start = i;
					if (matched == feature.Length) {
						return (i + buffer.Length + 1 - feature.Length) % buffer.Length;
					}
				} else {
					matched = 0;
				}
			}
			return -1;
		}

		public T[] DeQueue (T[] feature)
		{
			int start = FirstOf (feature);
			if (start >= 0) {
				T[] result = DeQueue(0, start >= head ?  start - head  : start + buffer.Length - head);
				head = ( start + feature.Length) % buffer.Length;
				if(result.Length > 16)
				{
					Console.WriteLine("first:{0},last:{1}",head,tail);
				}
				return result;
			} else {
				return null;
			}
		}

		public T[] DeQueue (T[] startFeature, T[] endFeature)
		{
			int start = FirstOf (startFeature);
			if (start >= 0) {
				int end = FirstOf(endFeature,start);
				if(end >= 0)
				{
					T[] result = DeQueue(start >= head ? start - head : start + buffer.Length - head,
					                     start <= end ? end - start : end + buffer.Length - start);
					head = (end + endFeature.Length) % buffer.Length;
					return result;
				}else{
					return null;
				}
			} else {
				return null;
			}
		}

		public T At(int index)
		{
			return this.buffer[(head + index) % buffer.Length];
		}

		#endregion

		#region IUnique implementation

		public long ID {
			get ;
			set ;
		}

		public override string ToString ()
		{
			return System.Text.ASCIIEncoding.Default.GetString(( Slice() as byte[]));
		}

		#endregion

		public RingBufferQueue (int defaultSize = 1001024)
		{
			buffer = new T[defaultSize];
			this.ID = globalSequenceNumber++;
		}

		public int Length
		{
			 get
			{
				if (head == tail)
					return 0;
				else if (head < tail)
					return tail - head;
				else
					return tail + buffer.Length - head;
			}
		}

		private int RemainSize()
		{
			return buffer.Length - this.Length - 1;
		}

		private void Expand(int size = -1)
		{
			int newBufferSize = size < 0 ? buffer.Length * 2 : buffer.Length + size;
			T[] newBuffer = new T[newBufferSize];
			if (head <= tail) {
				Array.Copy (buffer, head, newBuffer, 0, tail - head);
			} else {
				Array.Copy (buffer, head, newBuffer, 0, buffer.Length - head);
				Array.Copy (buffer, 0, newBuffer, buffer.Length - head, tail);
			}
			head = 0;
			tail = this.Length;
			this.buffer = newBuffer;
		}

		private T[] buffer;
		private int head = 0 ,tail = 0;
		private static int globalSequenceNumber = 1;
	}
}

