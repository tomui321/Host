namespace WebServer.Listeners
{
    public abstract class IListener
    {
        public bool Running;
        public abstract void Start();
        public abstract void Stop();
        public abstract object Accept();
    }
}
