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
            RRPCSetupEntrance superMain = new RRPCSetupEntrance((unity) => {
                //unity.RegisterSingleton<IADD,ADD>();
            }, (unity) => {
                //unity.AddServer<IADD, ADD>();
            }, (unitytoo) => {
            unitytoo.AddForwardingRequestNamespace("IRPCService", (x) => x.FirstOrDefault());
            });

            while ("q" != Console.ReadLine())
            {
                var session = RRPCServer.RRPCServerList.FirstOrDefault().Value.GetAllSessions().FirstOrDefault();
                var add = container.GetServices<IADD>(session);
                Parallel.For(0, 1000, (A) =>
                {
                    if (session != null)
                    {
                        //container.GetServices<IADD>(session).AsyncMM();
                        Console.WriteLine(A+"/1000");
                    }
                    else
                    {
                        Console.WriteLine("没有可以发送的对象");
                    }

                });
               
            }

        }
    }
}
