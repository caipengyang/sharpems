using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEMS.Net
{
    public class Add : CommandBase<SharpSession, StringRequestInfo>
    {
        public override void ExecuteCommand(SharpSession session, StringRequestInfo requestInfo)
        {
            session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
        }
    }
}
