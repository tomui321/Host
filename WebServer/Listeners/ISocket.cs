namespace WebServer.Listeners
{
    public interface ISocket
    {
        object GetSocket();
        void Close();
    }
}
