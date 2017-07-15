namespace WebServer.Listeners
{
    public abstract class Listener
    {
        public bool Running;
        public abstract void Start();
        public abstract void Stop();
        public abstract ISocket AcceptSocket();
    }
}
