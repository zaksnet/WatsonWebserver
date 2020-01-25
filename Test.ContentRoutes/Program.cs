using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WatsonWebserver;

namespace Test
{
    class Program
    {
        static void Main()
        {
            Server s = new Server("127.0.0.1", 9000, false, DefaultRoute); 
            s.ContentRoutes.Add("/", true);
            s.ContentRoutes.Add("/html/", true);
            s.ContentRoutes.Add("/large/", true);
            s.ContentRoutes.Add("/img/watson.jpg", false);

            s.Events.ExceptionEncountered = ExceptionEncountered;
            s.Events.RequestorDisconnected = RequestorDisconnected;

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        static bool AccessControlDenied(string ip, int port, string method, string url)
        {
            Console.WriteLine("AccessControlDenied [" + ip + ":" + port + "] " + method + " " + url);
            return true;
        }

        static bool ConnectionReceived(string ip, int port)
        {
            Console.WriteLine("ConnectionReceived [" + ip + ":" + port + "]");
            return true;
        }

        static bool ExceptionEncountered(string ip, int port, Exception e)
        {
            Console.WriteLine("ExceptionEncountered [" + ip + ":" + port + "]: " + Environment.NewLine + e.ToString());
            return true;
        }

        static bool RequestReceived(string ip, int port, string method, string url)
        {
            Console.WriteLine("RequestReceived [" + ip + ":" + port + "] " + method + " " + url);
            return true;
        }

        static bool ResponseSent(string ip, int port, string method, string url, int status, double totalTimeMs)
        {
            Console.WriteLine("ResponseSent [" + ip + ":" + port + "] " + method + " " + url + " status " + status + " " + totalTimeMs + "ms");
            return true;
        }

        static async Task DefaultRoute(HttpContext ctx)
        {
            ctx.Response.StatusCode = 200;
            await ctx.Response.Send("Default route");
            return; 
        }

        static void ExceptionEncountered(string ip, int port, Exception e)
        {
            Console.WriteLine("ExceptionEncountered [" + ip + ":" + port + "]: " + Environment.NewLine + e.ToString());
        }

        static void RequestorDisconnected(string ip, int port, string method, string url)
        {
            Console.WriteLine("RequestorDisconnected [" + ip + ":" + port + "]: " + method + " " + url);
        }
    }
}
