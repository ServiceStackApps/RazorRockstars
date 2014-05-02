using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ServiceStack;
using ServiceStack.Text;

namespace RazorRockstars.CompiledViews.WpfHost
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ExportMonoSqliteDll();

            new AppHost()
                .Init()
                .Start("http://*:1337/");
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
    }
}
