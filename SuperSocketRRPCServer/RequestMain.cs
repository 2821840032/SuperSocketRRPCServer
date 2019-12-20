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
           
           var info = JsonConvert.DeserializeObject<RequestExecutiveInformation>(requestInfo.bodyMeg);
            if (info.ReturnValue != null && session.MethodIDs.TryGetValue(info.ID, out var action))
            {
                action.Invoke(info.ReturnValue);
                //得到执行结果
            }
            else {
                //接收RPC的请求
                var type = session.MyAppServer.container.GetService(info.FullName, out object executionObj);
                var methodType = type.GetMethod(info.MethodName);
                List<object> paraList = new List<object>();
                for (int i = 0; i < info.Arguments.Count; i++)
                {
                    var paras = methodType.GetParameters();
                    paraList.Add(JsonConvert.DeserializeObject(info.Arguments[i], paras[i].ParameterType));
                }
                info.ReturnValue =JsonConvert.SerializeObject(methodType.Invoke(executionObj, paraList.ToArray()));
                var msg = JsonConvert.SerializeObject(info);
                session.Send(msg);

            }
            
            //session.MethodIDs
            Console.WriteLine("Client:"+requestInfo.bodyMeg);
        }
    }
}
