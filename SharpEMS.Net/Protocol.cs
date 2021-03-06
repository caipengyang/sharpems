﻿/*
 * Created by SharpDevelop.
 * User: yautacai
 * Date: 2014/1/12
 * Time: 18:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using SharpEMS.Util;

namespace SharpEMS.Net
{
	
	public enum SerType
	{
		Result     = 1,
		Param      = 2,
		All    	   = 3,
	}
	/// <summary>
	/// Description of Protocol.
	/// </summary>
	public class Protocol<T> : Singleton<Protocol<T> >
	{
		private  Polenter.Serialization.SharpSerializer serilizer = new Polenter.Serialization.SharpSerializer();
		
		//每一个完整包的分隔符
		public static byte[] PacketSeparator = new byte[] {(byte)('$'),(byte)('$'),(byte)('$'),(byte)('$')};
		
		private string []itemSeprator = new string[]{"@$@"};
		
		private byte[] buffer = new byte[1024 * 1024]; //max buffer size.
		
		private System.IO.Stream stream = new System.IO.MemoryStream();
		
		public void SetItemSeprator(string itemSeprator)
		{
			if(!itemSeprator.Equals(string.Empty))
			{
				this.itemSeprator[0] = itemSeprator;
			}
		}
		
		/// <summary>
		/// encode method name and parameters.
		/// </summary>
		/// <param name="method"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public byte[] Serialize(MethodRunTimeInfo info,SerType type = SerType.Param)
		{
			 System.Text.StringBuilder sb = new System.Text.StringBuilder();
			 sb.Append(info.MethodName);
			 int length = 0;
			 if(type != SerType.Param)
			 {
			 	stream.SetLength(0);
			 	stream.Seek(9,System.IO.SeekOrigin.Begin);
			 	serilizer.Serialize(info.ReturnValue,stream);
			 	stream.Seek(9,System.IO.SeekOrigin.Begin);
			 	length = stream.Read(buffer,0,(int)stream.Length);
			 	if(length < buffer.Length)
			 	{
			 		sb.Append(itemSeprator[0] + System.Text.ASCIIEncoding.Default.GetString(buffer,0,length));
			 	}else{
			 		SharpLog.Instance().LogErr("data size is more than 1m. not supported!");
			 	}
			 }
			 if(type != SerType.Result)
			 {
			 	 foreach(object obj in info.Params)
				 {
				 	stream.SetLength(0);
				 	stream.Seek(9,System.IO.SeekOrigin.Begin);
				 	serilizer.Serialize(obj,stream);
				 	stream.Seek(9,System.IO.SeekOrigin.Begin);
				 	length = stream.Read(buffer,0,(int)stream.Length);
				 	if(length < buffer.Length)
				 	{
				 		sb.Append(itemSeprator[0] + System.Text.ASCIIEncoding.Default.GetString(buffer,0,length));
				 	}else{
				 		SharpLog.Instance().LogErr("data size is more than 1m. not supported!");
				 	}
				 }
			 }
			 return System.Text.ASCIIEncoding.Default.GetBytes(sb.ToString());
		}
		
		public void Deserialize(byte[] buffer,MethodRunTimeInfo info,SerType type = SerType.Param)
		{
			string packet = System.Text.ASCIIEncoding.Default.GetString(buffer);
			string[] array = packet.Split(itemSeprator,StringSplitOptions.RemoveEmptyEntries);
			info.MethodName = array[0];
			if (type != SerType.Result) {
				info.Params = new object[array.Length - 1];
				for (int i = 1; i < array.Length; ++i) {
					stream.SetLength (0);
					stream.Seek (9, System.IO.SeekOrigin.Begin);
					byte[] data = System.Text.ASCIIEncoding.Default.GetBytes (array [i]);
					stream.Write (data, 0, data.Length);
					info.Params [i - 1] = serilizer.Deserialize (stream);
				}
			}
			if (type != SerType.Param) {
				stream.SetLength (0);
				stream.Seek (9, System.IO.SeekOrigin.Begin);
				byte[] data = System.Text.ASCIIEncoding.Default.GetBytes (array [1]);
				stream.Write (data, 0, data.Length);
				info.ReturnValue = serilizer.Deserialize (stream);
			}
		}
		
		protected void InvokeTarget(T target,MethodRunTimeInfo info)
		{
			try{
			info.ReturnValue = target.GetType ().InvokeMember (info.MethodName, System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
				null, target, info.Params);
			}catch(Exception ex) {
				SharpLog.Instance ().LogErr ("invoke server method occured exception.\nmessage:{0}\nstack trace:{1}", ex.Message, ex.StackTrace);
			}
		}
		
		public void DeserializeAndInvoke(T target,byte[] buffer,MethodRunTimeInfo info)
		{
			Deserialize (buffer, info);
			InvokeTarget (target, info);
			SharpLog.Instance ().LogDebug (" invoke server method returned:{0}", info.ReturnValue);
		}
		
	}
}
