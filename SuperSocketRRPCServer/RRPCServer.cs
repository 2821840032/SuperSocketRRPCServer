using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocketRRPCServer.CommunicationEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocketRRPCUnity;
using System.Reflection;
using SuperSocketRRPCServer.Entity;

namespace SuperSocketRRPCServer
{
    /// <summary>
    /// 基础服务
    /// </summary>
    public class RRPCServer : AppServer<RRPCSession, RequestBaseInfo>
    {
        /// <summary>
        /// 容器对象
        /// </summary>
       public UnityInIt container { get; private set; } 

        /// <summary>
        /// 所有的ServerList服务
        /// </summary> 
        public static List<RRPCServer> MyServerList { get; private set; } = new List<RRPCServer>();

       
        #region 反射设置值的对象列表

        /// <summary>
        ///  BaseProvideServices FulleName 
        /// </summary>
        public string BaseProvideServicesFullName { get;private set; }

        /// <summary>
        /// Socket PropertyInfo
        /// </summary>
        public PropertyInfo socketPropertyInfo { get; private set; }

        /// <summary>
        /// info PropertyInfo
        /// </summary>
        public PropertyInfo infoPropertyInfo { get; private set; }

        /// <summary>
        /// requestInfo PropertyInfo
        /// </summary>
        public PropertyInfo requestInfoPropertyInfo { get; private set; }

        #endregion


        /// <summary>
        /// 通过配置文件安装服务从这里启动
        /// </summary>
        public RRPCServer()
            : base(new DefaultReceiveFilterFactory<RRPCReceiveFilter, RequestBaseInfo>())
        {

            Type baseProvideServicesType = typeof(BaseProvideServices);
            BaseProvideServicesFullName = baseProvideServicesType.FullName;

            socketPropertyInfo = baseProvideServicesType.GetProperty("Socket");
            infoPropertyInfo = baseProvideServicesType.GetProperty("Info");
            requestInfoPropertyInfo = baseProvideServicesType.GetProperty("RequestInfo");


            container = new UnityInIt(BaseProvideServicesFullName);

            RRPCSetupEntrance.WholeUnitys.Add(container);
            MyServerList.Add(this);
        }

        /// <summary>
        /// 启动
        /// </summary>
        protected override void OnStarted()
        {
            Console.WriteLine(string.Format("Socket启动成功：{0}:{1}", this.Config.Ip, this.Config.Port));
            Logger.Info("启动成功");
            //启动成功
        }
    }
}
