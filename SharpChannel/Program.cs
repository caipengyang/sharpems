using System;

namespace SharpChannel.Common
{
	class MainClass
	{
		public static void Main1 (string[] args)
		{
			IBufferQueue<byte> queue = new RingBufferQueue<byte> (16);
			byte[] startFeature = {(byte)('$'),(byte)('$'),(byte)('$'),(byte)('$')};
			byte[] endFeature = {(byte)('$'),(byte)('$'),(byte)('$'),(byte)('$')};
			while (true) {
				Console.WriteLine("Input your choice :1. add string. 2.parse by end feature. 3.parse by start and end feature.");
				string choice;
				choice = Console.ReadLine();
				switch(choice)
				{
				case "1":
					Console.WriteLine("Input line:");
					choice = Console.ReadLine();
					queue.EnQueue(System.Text.ASCIIEncoding.Default.GetBytes(choice));
					break;
				case "2":
					byte[] data = queue.DeQueue(startFeature);
					if(data == null)
					{
						Console.WriteLine("not a valid packet.");
					}
					else{
						Console.WriteLine(" data:{0}",System.Text.ASCIIEncoding.Default.GetString(data));
					}
					break;
				case "3":
					data = queue.DeQueue(startFeature,endFeature);
					if(data == null)
					{
						Console.WriteLine("not a valid packet.");
					}
					else{
						Console.WriteLine(" data:{0}",System.Text.ASCIIEncoding.Default.GetString(data));
					}
					break;
				case "4":
					Console.WriteLine("data:{0}",queue.ToString());
					break;
				}
			}
			//Console.WriteLine ("Hello World!");
		}
	}
}
