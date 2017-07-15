using Helpers;
using Microsoft.Practices.Unity;
using PageLoader;

namespace Test
{
    public static class TypeRegistrations
    {
        public static UnityContainer Register()
        {
            var container = new UnityContainer();
            
            container.RegisterType<IConfigurationHelper, ConfigurationHelper>();
            container.RegisterType<IPageLoader, PageLoader.SimplePageLoader>();
            WebServer.TypeRegistrations.Register(container);

            return container;
        }
    }
}
