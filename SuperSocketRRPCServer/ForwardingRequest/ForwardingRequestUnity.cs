using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using System.Linq;
using System.Reflection;

namespace SuperSocketRRPCServer
{
    /// <summary>
    /// 用于转发请求的存储容器
    /// </summary>
   public class ForwardingRequestUnity
    {
        /// <summary>*
        /// 存储的FullName 转发标识列表
        /// </summary>
        public Dictionary<string, ForwardingRequestEnity> ForwardingRequestunity { get; set; }

        /// <summary>
        /// 初始化转发容器
        /// </summary>
        public ForwardingRequestUnity() {
            ForwardingRequestunity = new Dictionary<string, ForwardingRequestEnity>();
        }
        /// <summary>
        /// 添加转发服务规则-某个指定服务
        /// </summary>
        /// <typeparam name="IT">待转发请求的接口</typeparam>
        /// <param name="SelectRRPCServer">选择转发的服务器 不能为空</param>
        /// <param name="SelectRRPCSession">选择转发session 若为空则从列表中随机选取一个</param>
        public void AddForwardingRequestServer<IT>(Func<List<RRPCServer>, RRPCServer> SelectRRPCServer, Func<IEnumerable<RRPCSession>, RRPCSession> SelectRRPCSession=null)
        {
            if (SelectRRPCServer==null)
            {
                throw new Exception("SelectRRPCServer 项不能为空");
            }
            ForwardingRequestunity.Add(typeof(IT).FullName, new ForwardingRequestEnity() { ID = typeof(IT).FullName, SelectRRPCServer = SelectRRPCServer, SelectRRPCSession = SelectRRPCSession });
        }
        /// <summary>
        /// 添加转发服务规则-某个命名空间
        /// </summary>
        /// <param name="RRPCInterfaceName">远程接口 名称</param>
        /// <param name="SelectRRPCServer">选择转发的服务器 不能为空</param>
        /// <param name="SelectRRPCSession">选择转发session 若为空则从列表中随机选取一个</param>
        public void AddForwardingRequestNamespace(string RRPCInterfaceName, Func<List<RRPCServer>, RRPCServer> SelectRRPCServer, Func<IEnumerable<RRPCSession>, RRPCSession> SelectRRPCSession = null)
        {

            if (SelectRRPCServer == null)
            {
                throw new Exception("SelectRRPCServer 项不能为空");
            }

            var assembly = Assembly.Load(RRPCInterfaceName);
            if (assembly == null)
            {
                throw new Exception("没有找到此项目"+RRPCInterfaceName);
            }
            ForwardingRequestunity.Add(assembly.FullName, new ForwardingRequestEnity() { ID = assembly.FullName, SelectRRPCServer = SelectRRPCServer, SelectRRPCSession = SelectRRPCSession });
        }

        /// <summary>
        /// 查询转发的请求
        /// </summary>
        /// <param name="fullName">标识</param>
        /// <param name="assemblyFullName">程序集FullName</param>
        /// <param name="session">查询到的session</param>
        /// <param name="RRPCServers">服务列表</param>
        /// <returns></returns>
        public bool GetService(string fullName,string assemblyFullName, List<RRPCServer> RRPCServers, out RRPCSession session) {
            ForwardingRequestEnity value;
            if (ForwardingRequestunity.TryGetValue(fullName, out value)|| ForwardingRequestunity.TryGetValue(assemblyFullName, out value))
            {
                var  rrpcServer = value.SelectRRPCServer(RRPCServers);

                if (rrpcServer == null)
                {
                    RRPCServer.RRPCServerList.FirstOrDefault().Value.Log($"转发请求失败{fullName} 无法找到指定的RRPCServer 没有找到转发该请求的服务配置", LoggerType.Error);
                    session = null;
                    return false; ;
                }
                var count = rrpcServer.GetAllSessions().Count();
                if (count==0)
                {
                    rrpcServer.Log($"转发请求失败{fullName} 由于{rrpcServer.Config.Name} 连接客户端为0", LoggerType.Error);
                    session = null;
                    return false;
                }
                if (value.SelectRRPCSession == null)
                {
                    Random rm = new Random();
                    session = rrpcServer.GetAllSessions().Skip(rm.Next(count)).FirstOrDefault();
                }
                else {
                    session = value.SelectRRPCSession(rrpcServer.GetAllSessions());
                    if (session==null)
                    {
                        Random rm = new Random();
                        session = rrpcServer.GetAllSessions().Skip(rm.Next(rrpcServer.GetAllSessions().Count())).FirstOrDefault();
                    }
                }
                return true;
            }
            else {
                session = null;
                return false;
            }
        }
    }
    /// <summary>
    /// 存储转发请求的标识实体
    /// </summary>
    public class ForwardingRequestEnity {
        /// <summary>
        /// 存储接口标识
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 选择RRPCServer函数
        /// </summary>
        public Func<List<RRPCServer>, RRPCServer> SelectRRPCServer { get; set; }

        /// <summary>
        /// 选择RRPCSession函数
        /// </summary>
        public Func<IEnumerable<RRPCSession>, RRPCSession> SelectRRPCSession { get; set; }
    }
}
