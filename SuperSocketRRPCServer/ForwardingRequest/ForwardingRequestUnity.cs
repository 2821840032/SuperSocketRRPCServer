using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using System.Linq;
namespace SuperSocketRRPCServer
{
    /// <summary>
    /// 用于转发请求的存储容器
    /// </summary>
   public class ForwardingRequestUnity
    {
        /// <summary>
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
        /// 添加转发服务
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
        /// 查询转发的请求
        /// </summary>
        /// <param name="fullName">标识</param>
        /// <param name="RRPCServers">服务查询函数</param>
        /// <param name="session">查询到的session</param>
        /// <returns></returns>
        public bool GetService(string fullName, RRPCServer rrpcServer, out RRPCSession session) {
            if (ForwardingRequestunity.TryGetValue(fullName, out var value))
            {
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
