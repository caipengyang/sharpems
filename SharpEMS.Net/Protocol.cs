/*
 * Created by SharpDevelop.
 * User: yautacai
 * Date: 2014/1/12
 * Time: 18:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace SharpEMS.Net
{
	/// <summary>
	/// Description of Protocol.
	/// </summary>
	public class Protocol<T>
	{
		private  Polenter.Serialization.SharpSerializer serilizer = new Polenter.Serialization.SharpSerializer();
		
		//每一个完整包的分隔符
		public static byte[] PacketSeparator = new byte[] {(byte)('$'),(byte)('$'),(byte)('$'),(byte)('$')};
		
		private string []itemSeprator = new string[]{"@$@"};
		
		public Protocol(string itemSeprator = "")
		{
			if(!itemSeprator.Equals(string.Empty))
			{
				this.itemSeprator = itemSeprator;
			}
		}
		
		public Object Decode(byte[] bytes)
		{
			string packet = System.Text.ASCIIEncoding.Default.GetString(bytes);
			string[] array = packet.Split(itemSeprator,StringSplitOptions.RemoveEmptyEntries);
			
		}
		
		public
	}
}
