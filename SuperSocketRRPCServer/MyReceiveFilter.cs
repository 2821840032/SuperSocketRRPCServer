using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using SuperSocketRRPCServer.CommunicationEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketRRPCServer
{
    public class MyReceiveFilter : FixedHeaderReceiveFilter<RequestBaseInfo>
    {
        public MyReceiveFilter()
       : base(4)
        {

        }
         
        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            var headerData = new byte[4];
            Array.Copy(header, offset, headerData, 0, 4);
            return BitConverter.ToInt32(headerData, 0);
        }

        protected override RequestBaseInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return new RequestBaseInfo("RequestMain", bodyBuffer.CloneRange(offset, length));
        }
    }
}
