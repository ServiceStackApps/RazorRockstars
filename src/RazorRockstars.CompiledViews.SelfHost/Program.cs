using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Text;

namespace RazorRockstars.CompiledViews.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //LogManager.LogFactory = new ConsoleLogFactory();

            new AppHost()
                .Init()
                .Start("http://*:3333/");

            "\n\nListening on http://*:3333/..".Print();
            "Type Ctrl+C to quit..".Print();
            Process.Start("http://localhost:3333/");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
