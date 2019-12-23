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
using Unity;
namespace BTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //AOP容器对象
            AOPContainer container = new AOPContainer();
            //socket对象
            RRPCSetupEntrance superMain = new RRPCSetupEntrance((unity)=> {
                //unity.RegisterSingleton<IADD,ADD>();
            },(unity)=> { 
                //unity.AddServer<IADD, ADD>();
            });

            while ("q" != Console.ReadLine())
            {
                var session = RRPCServer.RRPCServerList.FirstOrDefault().Value.GetAllSessions().FirstOrDefault();
                var add = container.GetServices<IADD>(session);
                if (session != null)
                {
                    Console.WriteLine(container.GetServices<IADD>(session).GetRequestInfo(Guid.NewGuid()));
                }
                else
                {
                    Console.WriteLine("没有可以发送的对象");
                }
            }

        }
    }
}
