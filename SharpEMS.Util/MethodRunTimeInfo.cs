/*
 * Created by SharpDevelop.
 * User: yautacai
 * Date: 2014/1/12
 * Time: 19:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace SharpEMS.Util
{
	/// <summary>
	/// Description of MethodRunTimeInfo.
	/// </summary>
	public class MethodRunTimeInfo
	{
		public MethodRunTimeInfo()
		{
		}
		
		public Object ReturnValue
		{
			set;
			get;
		}
		
		public Object[] Params
		{
			get;
			set;
		}
		
		public String MethodName
		{
			get;
			set;
		}
	}
}
