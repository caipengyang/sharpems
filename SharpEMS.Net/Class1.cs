using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEMS.Net
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            var appServer = new SharpServer();
            if(!appServer.Setup(2012))
            {
                Console.WriteLine("setup server failed.");
                Console.ReadKey();
                return;
            }

            appServer.NewSessionConnected += appServer_NewSessionConnected;

            //appServer.NewRequestReceived += appServer_NewRequestReceived;

            if(!appServer.Start())
            {
                Console.WriteLine("start server failed.");
                Console.ReadKey();
                return;
            }

            while(Console.ReadLine().ToUpper() != "EXIT")
            {

            }
            Console.WriteLine("stop server.");
            appServer.Stop();
        }

        static void appServer_NewSessionConnected(SharpSession session)
        {
            Console.WriteLine("client connected.");
        }
    }
}
