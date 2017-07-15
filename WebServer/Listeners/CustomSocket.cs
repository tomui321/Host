using System.Net.Sockets;

namespace WebServer.Listeners
{
    internal class CustomSocket : ISocket
    {
        private readonly Socket _socket;

        public CustomSocket(Socket socket)
        {
            _socket = socket;
        }

        public object GetSocket()
        {
            return _socket;
        }

        public void Close()
        {
            _socket.Close();
            _socket.Dispose();
        }
    }
}
