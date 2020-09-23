using System;
using mike_and_conquer.externalcontrol;


namespace mike_and_conquer.main
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        // public static IDisposable restServer;
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

                RestServerManager.Initialize();
                MikeAndConquerGame game = new MikeAndConquerGame(testMode);
                game.Run();
//                using (var game = new MikeAndConquerGame(testMode))
//                    game.Run();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

        }
    }
}




