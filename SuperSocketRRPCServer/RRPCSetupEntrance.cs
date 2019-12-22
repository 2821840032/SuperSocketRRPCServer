using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using SuperSocketRRPCServer.CommunicationEntity;
using SuperSocketRRPCUnity;
using Unity;

namespace SuperSocketRRPCServer
{
    /// <summary>
    /// RRPC初始化入口
    /// </summary>
    public class RRPCSetupEntrance
    {
        /// <summary>
        /// 单例的入口类
        /// </summary>
        internal static RRPCSetupEntrance Single { get; private set; }
        /// <summary>
        /// 全局容器对象注入函数
        /// 在此处注入则所有的AppServer都会调用此函数
        /// </summary>
        internal Action<IUnityContainer> GlobalContainerInjection { get;private set; }

        /// <summary>
        /// 全局服务注入函数
        /// 在此处注入则所有的AppServer都会调用此函数
        /// </summary>
        public Action<UnityInIt<RRPCSession, RequestExecutiveInformation, RequestBaseInfo>> WholeUnitys { get; private set; }

        /// <summary>
        /// RRPC初始化入口
        /// </summary>
        public RRPCSetupEntrance(Action<IUnityContainer> GlobalContainerInjection, Action<UnityInIt<RRPCSession, RequestExecutiveInformation, RequestBaseInfo>> WholeUnitys)
        {
            if (Single!=null)
            {
                throw new Exception("已经初始化过一次RRPC");
            }
            Single = this;
            this.GlobalContainerInjection = GlobalContainerInjection;
            this.WholeUnitys = WholeUnitys;

            SetupAPPServers();
        }

        /// <summary>
        /// 启动监听服务
        /// </summary>
        private void SetupAPPServers() {
            var bootstrap = BootstrapFactory.CreateBootstrap();
            if (!bootstrap.Initialize())
            {
                Console.WriteLine("初始化失败");
                Console.ReadKey();
                return;
            }

            var result = bootstrap.Start();

            Console.WriteLine("Start result: {0}!", result);

            if (result == StartResult.Failed)
            {
                Console.WriteLine("程序没有被启动");
                Console.ReadKey();
                return;
            }
        }
    }
}
