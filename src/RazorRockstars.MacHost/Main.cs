using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Text;


namespace RazorRockstars.MacHost
{
    class MainClass
    {
        static void Main (string[] args)
        {
            new AppHost()
                .Init()
                .Start("http://*:1337/");

            NSApplication.Init ();
            NSApplication.Main (args);
        }
    }
}

