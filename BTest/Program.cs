using SuperSocketRRPCAOPContainer;
using SuperSocketRRPCServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //superMain.AddServer<IADD, ADD>();
            while (true)
            {
                Console.ReadLine();
                var session = MyServer.MyServerList.FirstOrDefault().GetAllSessions().FirstOrDefault();
                //var add = container.GetServices<IADD>(session);
                if (session != null)
                {
                    try
                    {
                        //add.AddArray(new string[] { "1", "2", "3" }, new string[] { "4", "5" });
                        //add.ADD(1,7);
                        //add.AddString("are you ", " ok ");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    //Console.WriteLine(.Result);
                }
                else
                {
                    Console.WriteLine("没有可以发送的对象");
                }
            }

        }
    }
}
