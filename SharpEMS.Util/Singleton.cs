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
	/// Description of Singleton.
	/// </summary>
	public class Singleton<T> 
		where T : new()
	{
		public Singleton()
		{
		}
		
		public static T Instance()
		{
			if(__instance == null) __instance = new T();
			return __instance;
		}
		
		private static T __instance ;
	}
}
