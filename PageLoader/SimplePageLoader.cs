using System.IO;
using Helpers;
using Helpers.Resources;

namespace PageLoader
{
    public class SimplePageLoader : IPageLoader
    {
        private readonly IConfigurationHelper _configurationHelper;

        public SimplePageLoader(IConfigurationHelper configurationHelper)
        {
            _configurationHelper = configurationHelper;
        }

        public string LoadContentFromLocalFile(string path)
        {
            try
            {
                var pathToFiles = _configurationHelper.GetFilesPath();

                if (string.IsNullOrEmpty(path))
                    path = Constants.IndexPage;

                var fullPath = Path.Combine(pathToFiles, path);

                if (!File.Exists(fullPath))
                {
                    return DefaultPages.Error404;
                }

                return File.ReadAllText(fullPath);
            }
            catch
            {
                return DefaultPages.Error500;
            }
        }
    }
}
