using System.Net.Sockets;
using Helpers;

namespace WebServer.Listeners
{
    public class CustomTcpListener : Listener
    {
        private readonly TcpListener _listener;

        public CustomTcpListener(IConfigurationHelper configurationHelper)
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
            _listener.Server.Close(0);
            _listener.Stop();
            Running = false;
        }

        public override ISocket AcceptSocket()
        {
            return new CustomSocket(_listener.AcceptSocket());
        }
    }
}
