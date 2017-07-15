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
        private readonly IListener _listener;
        private readonly IPageLoader _pageLoader;
        
        public SocketWebServer(IPageLoader pageLoader, IListener listener)
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
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (_listener.Running)
                {
                    var acceptedSocket = _listener.Accept();

                    var socket = acceptedSocket as Socket;

                    if (socket == null)
                        return;

                    ThreadPool.QueueUserWorkItem(sct =>
                    {
                        try
                        {
                            using (var stream = new NetworkStream(socket))
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    var request = GetRequest(reader);

                                    if (request == null)
                                        return;

                                    Respond(stream, request);
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            /* log or something in the future */
                        }
                        finally
                        {
                            socket.Close();
                        }
                    }, socket);
                }
            });
        }

        private SimpleProtocolRequest GetRequest(StreamReader reader)
        {
            var request = reader.ReadLine();

            return request == null ? null : JsonConvert.DeserializeObject<SimpleProtocolRequest>(request);
        }

        private void Respond(NetworkStream stream, SimpleProtocolRequest request)
        {
            using (var writer = new StreamWriter(stream) {AutoFlush = true})
            {
                var requestPath = request.Url.TrimStart(Path.AltDirectorySeparatorChar)
                    .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

                var responseContent = _pageLoader.LoadContentFromLocalFile(requestPath);

                writer.WriteLine(responseContent);
            }
        }
    }
}