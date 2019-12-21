using IRPCService;
using RPCService;
using SuperSocketRRPCAOPContainer;
using SuperSocketRRPCServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //AOP容器对象
            AOPContainer container = new AOPContainer();
            //socket对象
            RRPCSetupEntrance superMain = new RRPCSetupEntrance();
            superMain.AddServer<IADD, ADD>();

            while ("q" != Console.ReadLine())
            {
                var session = RRPCServer.MyServerList.FirstOrDefault().GetAllSessions().FirstOrDefault();
                var add = container.GetServices<IADD>(session);
                if (session != null)
                {
                    Console.WriteLine(container.GetServices<IADD>(session).GetRequestInfo());
                }
                else
                {
                    Console.WriteLine("没有可以发送的对象");
                }
            }

        }
    }
}
