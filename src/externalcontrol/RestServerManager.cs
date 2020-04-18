using System;
using Microsoft.Owin.Hosting;
using mike_and_conquer.main;

namespace mike_and_conquer.src.externalcontrol
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
