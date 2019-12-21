using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocketRRPCServer.CommunicationEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocketRRPCUnity;

namespace SuperSocketRRPCServer
{
    public class MyServer : AppServer<MySession, RequestBaseInfo>
    {
       public UnityInIt container { get; private set; } 

        /// <summary>
        /// 所有的ServerList服务
        /// </summary> 
        public static List<MyServer> MyServerList { get; private set; } = new List<MyServer>();

        /// <summary>
        /// 通过配置文件安装服务从这里启动
        /// </summary>
        public MyServer()
            : base(new DefaultReceiveFilterFactory<MyReceiveFilter, RequestBaseInfo>())
        {
            container = new UnityInIt();
            SuperMain.WholeUnitys.Add(container);
            MyServerList.Add(this);
        }

        protected override void OnStarted()
        {
            Console.WriteLine(string.Format("Socket启动成功：{0}:{1}", this.Config.Ip, this.Config.Port));
            Logger.Info("启动成功");
            //启动成功
        }
    }
}
