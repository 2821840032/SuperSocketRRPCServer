using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using SuperSocketRRPCUnity;

namespace SuperSocketRRPCServer
{
    /// <summary>
    /// RRPC初始化入口
    /// </summary>
    public class RRPCSetupEntrance
    {
        /// <summary>
        /// 全局的容器变量列表
        /// </summary>
        public static List<UnityInIt> WholeUnitys { get; set; }

        /// <summary>
        /// RRPC初始化入口
        /// </summary>
        public RRPCSetupEntrance()
        {
            WholeUnitys = new List<UnityInIt>();
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

        /// <summary>
        /// 为所有启动的服务器注册服务
        /// </summary>
        /// <typeparam name="IT"></typeparam>
        /// <typeparam name="T"></typeparam>
        public void AddServer<IT, T>()
           where IT : class
           where T : IT
        {
            foreach (var item in WholeUnitys)
            {
                item.AddServer<IT, T>();
            }
          
        }
    }
}
