using System;
using System.Reflection;

namespace SharpEMS.Util.Proxy
{
	/*
	 * Interface of proxy handler.
	*/
	public interface IProxyHandler
	{
		/**
		 * method : method to be called.
		 * args   : args passed.
		 * */
		object Invoke (MethodInfo method, Object[] args);
	}
}



