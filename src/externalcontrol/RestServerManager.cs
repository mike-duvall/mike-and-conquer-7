using System;
using Microsoft.Owin.Hosting;

namespace mike_and_conquer.externalcontrol
{
    public class RestServerManager
    {

        public static IDisposable restServer;

        public static void Initialize()
        {
            string baseAddress = "http://*:11369/";
            restServer = WebApp.Start<RestServerConfigurationProvider>(url: baseAddress);
        }

        public static void Shutdown()
        {
            restServer.Dispose();
        }

    }
}
