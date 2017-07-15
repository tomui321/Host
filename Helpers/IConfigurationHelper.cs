using System.Net;

namespace Helpers
{
    public interface IConfigurationHelper
    {
        string GetHost();
        string GetProtocol();
        int GetPort();
        IPAddress GetServer();
        string GetFilesPath();
    }
}
