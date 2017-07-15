using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Helpers;
using Newtonsoft.Json;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configurationHelper = new ConfigurationHelper();

            var port = configurationHelper.GetPort();

            // Make sure the server is running
            Thread.Sleep(4000);

            Console.WriteLine("Type the relative URL to sent a request or \'exit\' to quit.");

            var input = Console.ReadLine();

            while (input != "exit")
            {
                using (var client = new TcpClient(args[0], port))
                {
                    using (var stream = client.GetStream())
                    {
                        var reader = new StreamReader(stream);
                        var writer = new StreamWriter(stream) { AutoFlush = true };

                        var request = new SimpleProtocolClientRequest
                        {
                            Url = input
                        };

                        writer.WriteLine(JsonConvert.SerializeObject(request));
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }

                input = Console.ReadLine();
            }
        }
    }
}
