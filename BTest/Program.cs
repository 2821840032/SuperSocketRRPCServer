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
            SuperMain superMain = new SuperMain();
            superMain.AddServer<IADD, ADD>();

            while (true)
            {
                Test(container);
            }

        }
        static void Test(AOPContainer container) {
            Console.ReadLine();
            var session = MyServer.MyServerList.FirstOrDefault().GetAllSessions().FirstOrDefault();
            var add = container.GetServices<IADD>(session);
            if (session != null)
            {
                ADDTest(session, container);
            }
            else
            {
                Console.WriteLine("没有可以发送的对象");
            }
        }

         static void ADDTest(MySession client, AOPContainer Container)
        {
            Parallel.For(0, 5000, (id) =>
            {
                if (client.Connected)
                {
                    ActionAdd(client, Container);
                    Console.WriteLine($"执行程度{id}/5000");
                }
                else {
                    Console.WriteLine("客户端已关闭");
                }
              
            });
            Console.WriteLine("完成");
        }
        static int A = 0;
        static void ActionAdd(MySession client, AOPContainer Container)
        {
            var Ra1 = new Random().Next(10000);
            var Ra2 = new Random().Next(10000);
            try
            {
                var result = Container.GetServices<IADD>(client).ADD(Ra1, Ra2);
                if (result != (Ra1 + Ra2))
                {
                    Console.WriteLine("出现了一个异常的");
                    A++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
