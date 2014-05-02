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

            ExportMonoSqliteDll();

            new AppHost()
                .Init()
                .Start("http://*:1337/");

            "\n\nListening on http://*:1337/..".Print();
            "Type Ctrl+C to quit..".Print();
            Process.Start("http://localhost:1337/");

            Thread.Sleep(Timeout.Infinite);
        }

        public static void ExportMonoSqliteDll()
        {
            if (Env.IsMono)
                return; //Uses system sqlite3.so or sqlite3.dylib

            var resPath = "{0}.sqlite3.dll".Fmt(typeof(AppHost).Namespace);

            var resInfo = typeof(AppHost).Assembly.GetManifestResourceInfo(resPath);
            if (resInfo == null)
                throw new Exception("Couldn't load sqlite3.dll");

            var dllBytes = typeof(AppHost).Assembly.GetManifestResourceStream(resPath).ReadFully();
            var dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var filePath = Path.Combine(dirPath, "sqlite3.dll");

            File.WriteAllBytes(filePath, dllBytes);
        }

        public static void ExportWindowsSqliteDll()
        {
            var resPath = "{0}.{1}.SQLite.Interop.dll".Fmt(typeof(AppHost).Namespace, Environment.Is64BitProcess ? "x64" : "x86");

            var resInfo = typeof(AppHost).Assembly.GetManifestResourceInfo(resPath);
            if (resInfo == null)
                throw new Exception("Couldn't load SQLite.Interop.dll");

            var dllBytes = typeof(AppHost).Assembly.GetManifestResourceStream(resPath).ReadFully();

            var dirPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                               Environment.Is64BitProcess ? "x64" : "x86");

            var filePath = Path.Combine(dirPath, "SQLite.Interop.dll");

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            File.WriteAllBytes(filePath, dllBytes);
        }

    }
}
