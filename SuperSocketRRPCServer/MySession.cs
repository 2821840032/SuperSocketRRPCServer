using SuperSocket.SocketBase;
using SuperSocketRRPCServer.CommunicationEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketRRPCServer
{
    public class MySession : AppSession<MySession, RequestBaseInfo>
    {
        public MyServer MyAppServer => (MyServer)AppServer;
        /// <summary>
        /// 执行事件中的方法
        /// </summary>
        public Dictionary<Guid,Action<string>> MethodIDs { get; set; }
        public MySession()
        {
            MethodIDs = new Dictionary<Guid, Action<string>>();
        }
        protected override void OnSessionStarted()
        {

        }

        protected override void OnInit()
        {
            base.OnInit();
        }

        protected override void HandleUnknownRequest(RequestBaseInfo requestInfo)
        {
            Console.WriteLine("收到未知消息："+requestInfo.bodyMeg);
        }

        protected override void HandleException(Exception e)
        {

        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            base.OnSessionClosed(reason);
        }
        public override void Send(string message)
        {
            var dataBody = Encoding.UTF8.GetBytes(message);

            var dataLen = BitConverter.GetBytes(dataBody.Length);//int类型占4位，根据协议这里也只能4位，否则会出错

            var sendData = new byte[4 + dataBody.Length];//长度为4

            // +-------+-------------------------------+
            // |request|                               |
            // | name  |    request body               |
            // |  (4)  |                               |
            // |       |                               |
            // +-------+-------------------------------+

            Array.ConstrainedCopy(dataLen, 0, sendData, 0, 4);
            Array.ConstrainedCopy(dataBody, 0, sendData, 4, dataBody.Length);

            base.Send(sendData, 0, sendData.Length);
        }
    }
}
