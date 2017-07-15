using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using Helpers.Resources;

namespace Helpers
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public string GetHost()
        {
            return $"{GetProtocol()}://{GetServer()}:{GetPort()}/";
        }
        
        public string GetProtocol()
        {
            var protocol = ConfigurationManager.AppSettings["Protocol"];

            if (string.IsNullOrEmpty(protocol))
                throw new Exception(ErrorMessages.MissingProtocol);

            if (!(new[] { Constants.Http, Constants.Https }).ToList().Contains(protocol))
                throw new Exception(ErrorMessages.WrongProtocol);

            return protocol;
        }

        public int GetPort()
        {
            var port = ConfigurationManager.AppSettings["Port"];
            
            if (string.IsNullOrEmpty(port))
                throw new Exception(ErrorMessages.MissingPort);

            port = port.Trim();

            if (!port.All(char.IsDigit) || port.Length > 6 || port.Length == 0)
                throw new Exception(ErrorMessages.WrongPort);

            return Convert.ToInt32(port);
        }

        public IPAddress GetServer()
        {
            var server = ConfigurationManager.AppSettings["Server"];

            if (string.IsNullOrEmpty(server))
                throw new Exception(ErrorMessages.MissingServer);

            IPAddress ip;
            IPAddress.TryParse(server, out ip);

            if (ip == null)
            {
                throw new Exception(ErrorMessages.WrongServer);
            }

            return ip;
        }

        public string GetFilesPath()
        {
            var path = ConfigurationManager.AppSettings["PathToFiles"];

            if (string.IsNullOrEmpty(path))
                throw new Exception(ErrorMessages.MissingPathToFiles);

            if (!Directory.Exists(path))
                throw new Exception(ErrorMessages.PathToFilesDirectoryDoesNotExist);

            return path;
        }
    }
}
