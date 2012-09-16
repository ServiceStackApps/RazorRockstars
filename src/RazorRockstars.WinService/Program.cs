using System;
using System.Threading;
using ServiceStack.Text;

namespace RazorRockstars.WinService
{
    static class Program
    {
        public const string ListeningOn = "http://*:2002/";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var appHost = new AppHost();

            //Allow you to debug your Windows Service while you're deleloping it. 
#if DEBUG
            "Running WinServiceAppHost in Console mode".Print();
            try
            {
                appHost.Init();
                appHost.Start(ListeningOn);

                "Press <CTRL>+C to stop.".Print();
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                "ERROR: {0}: {1}".Print(ex.GetType().Name, ex.Message);
                throw;
            }
            finally
            {
                appHost.Stop();
            }
            "WinServiceAppHost has finished".Print();

#else
			//When in RELEASE mode it will run as a Windows Service with the code below

            var servicesToRun = new System.ServiceProcess.ServiceBase[] 
			{ 
				new WinService(appHost, ListeningOn) 
			};
            System.ServiceProcess.ServiceBase.Run(servicesToRun);
#endif

        }
    }
}
