using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using ServiceStack;
using ServiceStack.Text;

namespace RazorRockstars.CompiledViews.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //LogManager.LogFactory = new ConsoleLogFactory();
            ExportSqliteDll();

            new AppHost()
                .Init()
                .Start("http://*:1337/");

            "\n\nListening on http://*:1337/..".Print();
            "Type Ctrl+C to quit..".Print();
            Process.Start("http://localhost:1337/");

            Thread.Sleep(Timeout.Infinite);
        }

        public static void ExportSqliteDll()
        {
            var resPath = "{0}.{1}.SQLite.Interop.dll".Fmt(typeof(AppHost).Namespace, Environment.Is64BitProcess ? "x64" : "x86");

            var resInfo = typeof(AppHost).Assembly.GetManifestResourceInfo(resPath);
            if (resInfo == null)
                throw new Exception("Couldn't load SQLite.Interop.dll");

            var dllBytes = typeof(AppHost).Assembly.GetManifestResourceStream(resPath).ReadFully();

            //Always throws with either x86 / x64 dlls. Might succeed 1 day, but not this day.
            //try
            //{
            //    Assembly.Load(dllBytes);
            //    return;
            //}
            //catch (Exception) {}

            var dirPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                               Environment.Is64BitProcess ? "x64" : "x86");

            var filePath = Path.Combine(dirPath, "SQLite.Interop.dll");

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            File.WriteAllBytes(filePath, dllBytes);
        }

    }
}
