using System;

namespace SharpEMS.Net
{
	public class SocketProxyHandler : SharpEMS.Util.Singleton<SocketProxyHandler>,SharpEMS.Util.Proxy.IProxyHandler
	{
		#region IProxyHandler implementation

		public object Invoke (System.Reflection.MethodInfo method, object[] args)
		{
			methodInfo.MethodName = method.Name;
			methodInfo.Params = args;
			this.client.SendMessage (Protocol<SharpEMS.Util.Proxy.IProxyHandler>.Instance ().Serialize (methodInfo));
			return methodInfo.ReturnValue;
		}

		#endregion

		public SocketProxyHandler ()
		{
			this.client = new SharpChannel.Client.TCPClient ("127.0.0.1", 10086);
			this.methodInfo =  new SharpEMS.Util.MethodRunTimeInfo ();
			this.client.NewPacketReceived += (object sender, SharpChannel.NewPacketArgs args) => {
				Protocol<SharpEMS.Util.Proxy.IProxyHandler>.Instance ().Deserialize(args.Data,methodInfo,SerType.Result);
			};
		}
		private SharpEMS.Util.MethodRunTimeInfo methodInfo;
		private SharpChannel.Client.TCPClient client ;
	}
}

