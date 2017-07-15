using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Helpers.Resources;
using Newtonsoft.Json;
using PageLoader;
using WebServer.Dto;
using WebServer.Listeners;

namespace WebServer.Servers
{
    public class SocketWebServer : IWebServer
    {
        private readonly Listener _listener;
        private readonly IPageLoader _pageLoader;
        
        public SocketWebServer(IPageLoader pageLoader, Listener listener)
        {
            _pageLoader = pageLoader;
            _listener = listener;
        }

        public void Run()
        {
            if (!_listener.Running)
            {
                _listener.Start();
                HandleRequests();
            }
            else
            {
                throw new Exception(ErrorMessages.AlreadyRunning);
            }
        }

        public void Stop()
        {
            if (!_listener.Running)
                throw new Exception(ErrorMessages.ServerNotRunning);

            _listener.Stop();
        }

        private void HandleRequests()
        {
            if (!_listener.Running)
            {
                throw new Exception(ErrorMessages.ServerNotRunning);
            }

            ThreadPool.QueueUserWorkItem(o =>
            {
                Console.WriteLine(@"Socket web server running");

                while (true)
                {
                    ThreadPool.QueueUserWorkItem(sct =>
                    {
                        var client = sct as ISocket;

                        var socket = client?.GetSocket() as Socket;

                        if (socket == null)
                            return;

                        try
                        {
                            var stream = new NetworkStream(socket);
                            var reader = new StreamReader(stream);
                            var writer = new StreamWriter(stream) {AutoFlush = true};

                            var request = reader.ReadLine();

                            if (request == null)
                                return;

                            var deserializedRequest = JsonConvert.DeserializeObject<SimpleProtocolRequest>(request);

                            var requestPath = deserializedRequest.Url.TrimStart(Path.AltDirectorySeparatorChar)
                                .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

                            writer.WriteLine(_pageLoader.LoadContentFromLocalFile(requestPath));
                            stream.Close();
                        }
                        finally
                        {
                            client.Close();
                        }
                    }, _listener.AcceptSocket());
                }
            });
        }
    }
}