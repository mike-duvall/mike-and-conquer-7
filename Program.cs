using System;
using Microsoft.Owin.Hosting;
using System.Net.Http;

namespace mike_and_conquer
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            string baseAddress = "http://localhost:11369/";
            WebApp.Start<Startup>(url: baseAddress);
            using (var game = new MikeAndConqueryGame())
                game.Run();

            int x = 3;
        }
    }
}


