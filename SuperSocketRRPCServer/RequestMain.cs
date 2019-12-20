using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;
using SuperSocketRRPCServer.CommunicationEntity;

namespace SuperSocketRRPCServer
{
    /// <summary>
    /// 所有请求的入口
    /// </summary>
    public class RequestMain : CommandBase<MySession, RequestBaseInfo>
    {
        public override void ExecuteCommand(MySession session, RequestBaseInfo requestInfo)
        {

            RequestExecutiveInformation info = null;
            try
            {
                info = JsonConvert.DeserializeObject<RequestExecutiveInformation>(requestInfo.bodyMeg);
            }
            catch (Exception e)
            {
                Console.WriteLine("解析失败" + requestInfo.bodyMeg);
                return;
            }
            if (info.ReturnValue != null && session.RemoteCallQueue.GetTaskIDAndSuccess(info.ID, info.ReturnValue))
            {
                //处理完成
            }
            else if (info.ReturnValue != null)
            {
                Console.WriteLine($"收到一个意外的请求 它有结果但是没有找到该任务的信息 ID:{info.ID} FullName:{info.FullName} Return:{info.ReturnValue} 来自于:{session.RemoteEndPoint.ToString()}");
            }
            else
            {
                //接收RPC的请求
                AsyncImplementFunc(info, session, requestInfo);

            }
            
            //session.MethodIDs
            Console.WriteLine("Client:"+requestInfo.bodyMeg);
        }
        /// <summary>
        /// 执行RPC的调用
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        /// <returns></returns>
          async  Task AsyncImplementFunc(RequestExecutiveInformation info,MySession session, RequestBaseInfo requestInfo) {
            await Task.Yield();
            var type = session.MyAppServer.container.GetService(info.FullName, out object executionObj);
            var methodType = type.GetMethod(info.MethodName);
            List<object> paraList = new List<object>();
            for (int i = 0; i < info.Arguments.Count; i++)
            {
                var paras = methodType.GetParameters();
                paraList.Add(JsonConvert.DeserializeObject(info.Arguments[i], paras[i].ParameterType));
            }
            info.ReturnValue = JsonConvert.SerializeObject(methodType.Invoke(executionObj, paraList.ToArray()));
            var msg = JsonConvert.SerializeObject(info);
            session.Send(msg);
        }
    }
}
