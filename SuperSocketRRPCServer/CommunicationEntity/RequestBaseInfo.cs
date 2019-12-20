using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketRRPCServer.CommunicationEntity
{
    /// <summary>
    /// 最外部包装
    /// </summary>
    public class RequestBaseInfo : IRequestInfo
    {
        public string Key { get; set; }

        public byte[] body { get; set; }
        public RequestBaseInfo(string key, byte[] body) {
            this.Key = key;
            this.body = body;
        }
        public string bodyMeg => Encoding.UTF8.GetString(body); 
    }
}
