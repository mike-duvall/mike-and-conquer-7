using System;
using Microsoft.Owin.Hosting;
using System.Net.Http;

namespace mike_and_conquer_6
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
            using (var game = new Game1())
                game.Run();

            int x = 3;
        }
    }
}


//using Microsoft.Owin.Hosting;
//using System;
//using System.Net.Http;

//namespace OwinSelfhostSample
//{
//    public class Program
//    {
//        static void Main()
//        {
//            string baseAddress = "http://localhost:9000/";

//            // Start OWIN host 
//            using (WebApp.Start<Startup>(url: baseAddress))
//            {
//                // Create HttpCient and make a request to api/values 
//                HttpClient client = new HttpClient();

//                var response = client.GetAsync(baseAddress + "api/values").Result;

//                Console.WriteLine(response);
//                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
//                Console.ReadLine();
//            }
//        }
//    }
//}