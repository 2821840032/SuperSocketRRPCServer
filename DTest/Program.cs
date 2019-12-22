using IRPCService;
using RPCService;
using SuperSocketRRPCAOPContainer;
using SuperSocketRRPCServer;
using System;

namespace DTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //AOP容器对象
            AOPContainer container = new AOPContainer();
            //socket对象
            RRPCSetupEntrance superMain = new RRPCSetupEntrance((unity) => {
                //unity.RegisterType<IADD,ADD>();
            }, (unity) => {
                unity.AddServer<IADD, ADD>();
            });

            while ("q" != Console.ReadLine())
            {
                //var session = RRPCServer.MyServerList.FirstOrDefault().GetAllSessions().FirstOrDefault();
                //var add = container.GetServices<IADD>(session);
                //if (session != null)
                //{
                //    Console.WriteLine(container.GetServices<IADD>(session).GetRequestInfo());
                //}
                //else
                //{
                //    Console.WriteLine("没有可以发送的对象");
                //}
            }
        }
    }
}
