using System;
using Microsoft.Owin.Hosting;


namespace mike_and_conquer
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public static IDisposable restServer;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            try
            {
                bool testMode = false;
                if(args.Length > 0)
                {
                    if ("TESTMODE" == args[0])
                        testMode = true;
                }

                //string baseAddress = "http://localhost:11369/";
                string baseAddress = "http://*:11369/";
                restServer = WebApp.Start<Startup>(url: baseAddress);
                using (var game = new MikeAndConqueryGame(testMode))
                    game.Run();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

        }
    }
}




