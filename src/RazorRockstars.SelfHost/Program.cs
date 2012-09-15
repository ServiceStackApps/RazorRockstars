using System;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.Text;

namespace RazorRockstars.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.LogFactory = new ConsoleLogFactory();

            var appHost = new AppHost();
            appHost.Init();
            appHost.Start("http://*:2001/");

            "\n\nType any key to quit..".Print();
            Console.ReadKey();
        }
    }
}
