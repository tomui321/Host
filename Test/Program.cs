using System;
using Microsoft.Practices.Unity;
using WebServer;

namespace Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            var container = TypeRegistrations.Register();
            var webServer = container.Resolve<IWebServer>();
            
            webServer.Run();

            Console.WriteLine("Press a key to quit.");
            Console.ReadKey();

            webServer.Stop();
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}