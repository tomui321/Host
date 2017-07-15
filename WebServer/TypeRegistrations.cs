using Microsoft.Practices.Unity;
using WebServer.Listeners;

namespace WebServer
{
    public static class TypeRegistrations
    {
        public static void Register(UnityContainer container)
        {
            container.RegisterType<IWebServer, Servers.SocketWebServer>();
            container.RegisterType<Listener, CustomTcpListener>();
            container.RegisterType<ISocket, CustomSocket>();
        }
    }
}
