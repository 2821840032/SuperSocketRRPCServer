using SuperSocket.SocketBase;
using SuperSocketRRPCServer.CommunicationEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperSocketRRPCServer
{
    public class MySession : AppSession<MySession, RequestBaseInfo>
    {
        public MyServer MyAppServer => (MyServer)AppServer;

        /// <summary>
        /// 远程任务队列
        /// </summary>
        public RemoteCallQueue RemoteCallQueue { get; private set; }
        public MySession()
        {
            RemoteCallQueue = new RemoteCallQueue(10);
        }
        protected override void OnSessionStarted()
        {

        }

        protected override void OnInit()
        {
            base.OnInit();
            Console.WriteLine("新的连接："+SessionID);
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
            //释放所有的任务
            RemoteCallQueue.ErrorEmpty("远程对象被关闭 原因:"+reason);

            base.OnSessionClosed(reason);
        }
        public void SendMessage(string message)
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
