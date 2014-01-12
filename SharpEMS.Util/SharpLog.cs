/*
 * Created by SharpDevelop.
 * User: yautacai
 * Date: 2014/1/12
 * Time: 18:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace SharpEMS.Util
{
	/// <summary>
	/// Description of SharpLog.
	/// </summary>
	public class SharpLog : Singleton<SharpLog>
	{
		public enum LogLevel
		{
			DEBUG 	= 	1,
			INFO	=	2,
			WARN  	= 	3,
			ERROR 	= 	4,
		}
		
		public void Log(LogLevel level ,string format, params object[] param)
		{
			Console.WriteLine("log level :{0}\n.Message:{1}",level,string.Format(format,param));
		}
		
		public void LogDebug(string format,params object[] param)
		{
			Log(LogLevel.DEBUG,format,param);
		}
		
		public void LogErr(string format,params object[] param)
		{
			Log(LogLevel.ERROR,format,param);
		}
		
		public void LogInfo(string format,params object[] param)
		{
			Log(LogLevel.ERROR,format,param);
		}
				
		public void LogWarning(string format,params object[] param)
		{
			Log(LogLevel.ERROR,format,param);
		}
	}
}
