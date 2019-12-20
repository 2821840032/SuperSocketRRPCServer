using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using SuperSocketRRPCServer;
using SuperSocketRRPCServer.CommunicationEntity;

namespace SuperSocketRRPCAOPContainer
{

    public class AOPContainer
    {
        private string returnValueString;
        ProxyGenerator generator { get; set; }
        public AOPContainer()
        {
            generator = new ProxyGenerator();
        }
     

        /// <summary>
        /// 获取远程类
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="session">需要通讯的对象</param>
        /// <returns></returns>
        public T GetServices<T>(MySession session) where T:class
        {
            EventWaitHandle _waitHandle = new AutoResetEvent(false);
            return generator.CreateInterfaceProxyWithoutTarget<T>(new AOPRPCInterceptor((invocation) => {
                return ImplementFunc(invocation, session, _waitHandle);
            }));
        }
        Object ImplementFunc(IInvocation invocation, MySession session, EventWaitHandle waitHandle) {
            RequestExecutiveInformation information = new RequestExecutiveInformation()
            {
                FullName=invocation.Method.DeclaringType.FullName,
                ID=Guid.NewGuid(),
                MethodName=invocation.Method.Name,
                Arguments=invocation.Arguments.Select(d => JsonConvert.SerializeObject(d)).ToList()
            };
            session.MethodIDs.Add(information.ID, (reMsg) => {
                returnValueString = reMsg;
                waitHandle.Set();
            });
            var msg = JsonConvert.SerializeObject(information);
            session.Send(msg);
            waitHandle.WaitOne();
            session.MethodIDs.Remove(information.ID);

            var obj = JsonConvert.DeserializeObject(returnValueString, invocation.Method.ReturnType);
            return obj;
        }

        /// <summary>
        /// 得到调用结果后
        /// </summary>
        /// <param name="o"></param>
        public void ReceiveReturnValue(string msg) {
           
        }
    }
   
}
