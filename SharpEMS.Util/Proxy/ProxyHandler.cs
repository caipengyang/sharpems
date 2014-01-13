using System;
using System.Reflection;

namespace SharpEMS.Util.Proxy
{
	public interface IService
	{
		int add (int a, int b);
		int min (int a, int b);
		int tim (int a, int b);
		int div (int a, int b);
	}

	public class ProxyHandler : IProxyHandler
	{
		public object Invoke (/*Object sender,*/MethodInfo method, Object[] args)
		{
			switch (method.Name) {
			case "add":
				return (int)args [0] + (int)args [1];
			case "min":
				return (int)args [0] - (int)args [1];
			case "tim":
				return (int)args [0] * (int)args [1];
			case "div":
				return (int)args [0] / (int)args [1];
			default:
				return 0;
			}
		}
	}
}

