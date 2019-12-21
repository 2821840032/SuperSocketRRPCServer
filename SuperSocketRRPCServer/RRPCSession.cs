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
    /// <summary>
    /// Session
    /// </summary>
    public class RRPCSession : AppSession<RRPCSession, RequestBaseInfo>
    {
        /// <summary>
        /// 连接对象
        /// </summary>
        public RRPCServer RrpcAppServer => (RRPCServer)AppServer;

        /// <summary>
        /// 远程任务队列
        /// </summary>
        public RemoteCallQueue RemoteCallQueue { get; private set; }
        /// <summary>
        /// session
        /// </summary>
        public RRPCSession()
        {
            RemoteCallQueue = new RemoteCallQueue(10);
        }
        /// <summary>
        /// session连接加入
        /// </summary>
        protected override void OnSessionStarted()
        {
            Console.WriteLine("新的连接：" + SessionID);

        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();
            
        }
        /// <summary>
        /// 未知消息
        /// </summary>
        /// <param name="requestInfo"></param>
        protected override void HandleUnknownRequest(RequestBaseInfo requestInfo)
        {
            Console.WriteLine("收到未知消息："+requestInfo.bodyMeg);
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="e"></param>
        protected override void HandleException(Exception e)
        {

        }
        /// <summary>
        /// 连接断开
        /// </summary>
        /// <param name="reason"></param>
        protected override void OnSessionClosed(CloseReason reason)
        {
            Console.WriteLine($"远程连接断开 IP-Prot:{RemoteEndPoint.ToString()}");
            //释放所有的任务
            RemoteCallQueue.ErrorEmpty("远程对象被关闭 原因:"+reason);

            base.OnSessionClosed(reason);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
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
