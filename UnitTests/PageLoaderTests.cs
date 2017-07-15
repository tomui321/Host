using System;
using System.IO;
using Helpers;
using Helpers.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PageLoader;

namespace UnitTests
{
    [TestClass]
    public class PageLoaderTests
    {
        private SimplePageLoader _pageLoader;
        private Mock<IConfigurationHelper> _configurationHelperMock;

        [TestInitialize]
        public void Setup()
        {
            _configurationHelperMock = new Mock<IConfigurationHelper>();
            _configurationHelperMock.Setup(x => x.GetFilesPath())
                .Returns(Path.Combine(Environment.CurrentDirectory, @"Resources"));

            _pageLoader = new SimplePageLoader(_configurationHelperMock.Object);
        }

        [TestMethod]
        public void LoadContentFromLocalFile_CorrectPath_DesiredPageReturned()
        {
            var input = "SimplePage.html";

            var expectation = File.ReadAllText(@"Resources/SimplePage.html");
            var result = _pageLoader.LoadContentFromLocalFile(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectation, result);
        }

        [TestMethod]
        public void LoadContentFromLocalFile_NullPathIndexAvailable_IndexPageReturned()
        {
            var input = (string) null;

            var expectation = File.ReadAllText(@"Resources/Index.html");
            var result = _pageLoader.LoadContentFromLocalFile(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectation, result);
        }


        [TestMethod]
        public void LoadContentFromLocalFile_EmptyPathIndexAvailable_IndexPageReturned()
        {
            var input = string.Empty;

            var expectation = File.ReadAllText(@"Resources/Index.html");
            var result = _pageLoader.LoadContentFromLocalFile(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectation, result);
        }

        [TestMethod]
        public void LoadContentFromLocalFile_NullPathIndexNotAvailable_404PageReturned()
        {
            var localConfigurationHelperMock = new Mock<IConfigurationHelper>();
            localConfigurationHelperMock.Setup(x => x.GetFilesPath())
                .Returns(Path.Combine(Environment.CurrentDirectory, @"Resources/Empty"));

            var localPageLoader = new SimplePageLoader(localConfigurationHelperMock.Object);


            var input = (string) null;
            var expectation = DefaultPages.Error404;
            var result = localPageLoader.LoadContentFromLocalFile(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectation, result);
        }

        [TestMethod]
        public void LoadContentFromLocalFile_EmptyPathIndexNotAvailable_404PageReturned()
        {
            var localConfigurationHelperMock = new Mock<IConfigurationHelper>();
            localConfigurationHelperMock.Setup(x => x.GetFilesPath())
                .Returns(Path.Combine(Environment.CurrentDirectory, @"Resources/Empty"));

            var localPageLoader = new SimplePageLoader(localConfigurationHelperMock.Object);

            var input = string.Empty;
            var expectation = DefaultPages.Error404;
            var result = localPageLoader.LoadContentFromLocalFile(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectation, result);
        }

        [TestMethod]
        public void LoadContentFromLocalFile_WrongPath_404PageReturned()
        {
            var input = "Tralala.html";

            var expectation = DefaultPages.Error404;
            var result = _pageLoader.LoadContentFromLocalFile(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectation, result);
        }

        [TestMethod]
        public void LoadContentFromLocalFile_ExceptionOccurs_500PageReturned()
        {
            var localConfigurationHelperMock = new Mock<IConfigurationHelper>();
            localConfigurationHelperMock.Setup(x => x.GetFilesPath()).Throws(new Exception("Test exception"));

            var localPageLoader = new SimplePageLoader(localConfigurationHelperMock.Object);

            var input = string.Empty;
            var expectation = DefaultPages.Error500;
            var result = localPageLoader.LoadContentFromLocalFile(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectation, result);
        }
    }
}