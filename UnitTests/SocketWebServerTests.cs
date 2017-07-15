using System;
using System.IO;
using System.Net;
using Helpers;
using Helpers.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using PageLoader;
using WebServer.Dto;
using WebServer.Listeners;
using WebServer.Servers;

namespace UnitTests
{
    [TestClass]
    public class SocketWebServerTests
    {

        private SocketWebServer _server;
        private Mock<IConfigurationHelper> _configurationHelperMock;

        [TestInitialize]
        public void Setup()
        {
            _configurationHelperMock = new Mock<IConfigurationHelper>();
            _configurationHelperMock.Setup(x => x.GetServer()).Returns(IPAddress.Parse(Constants.Localhost));
            _configurationHelperMock.Setup(x => x.GetProtocol()).Returns(Constants.Http);

            var pageLoaderMock = new Mock<IPageLoader>();

            _server = new SocketWebServer(pageLoaderMock.Object, new ProxyTcpListener(_configurationHelperMock.Object));
        }
        
        [TestMethod]
        public void Run_Stop_Once()
        {
            _configurationHelperMock.Setup(x => x.GetPort()).Returns(12345);

            try
            {
                _server.Run();

                _server.Stop();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void Run_Stop_Twice()
        {
            _configurationHelperMock.Setup(x => x.GetPort()).Returns(12346);

            try
            {
                _server.Run();

                _server.Stop();

                _server.Run();

                _server.Stop();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void StopNonRunning_NotRunningException()
        {
            _configurationHelperMock.Setup(x => x.GetPort()).Returns(12347);

            try
            {
                _server.Stop();

                Assert.Fail("Exception expected!");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.ServerNotRunning, ex.Message);
            }
        }

        [TestMethod]
        public void RunTwice_AlreadyRunningException()
        {
            _configurationHelperMock.Setup(x => x.GetPort()).Returns(12348);

            _server.Run();

            try
            {
                _server.Run();

                Assert.Fail("Exception expected!");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.AlreadyRunning, ex.Message);
            }
        }

        [TestMethod]
        public void StopTwice_AlreadyStoppedException()
        {
            _configurationHelperMock.Setup(x => x.GetPort()).Returns(12349);

            _server.Run();

            try
            {
                _server.Stop();
                _server.Stop();

                Assert.Fail("Exception expected!");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.ServerNotRunning, ex.Message);
            }
        }
    }
}
