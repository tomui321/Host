using System;
using System.Net.Sockets;
using Helpers;

namespace WebServer.Listeners
{
    public class ProxyTcpListener : IListener
    {
        private readonly TcpListener _listener;

        public ProxyTcpListener(IConfigurationHelper configurationHelper)
        {
            var port = configurationHelper.GetPort();
            var server = configurationHelper.GetServer();

            _listener = new TcpListener(server, port);
        }

        public override void Start()
        {
            _listener.Start();
            Running = true;
        }

        public override void Stop()
        {
            Running = false;
            _listener.Server.Close(0);
            _listener.Stop();
        }

        public override object Accept()
        {
            return Running ? _listener.AcceptSocket() : null;
        }
    }
}
