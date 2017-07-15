using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Client;
using Helpers;
using Helpers.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using PageLoader;
using WebServer.Listeners;
using WebServer.Servers;

namespace IntegrationTests
{
    [TestClass]
    public class SocketWebServerTests
    {
        private Mock<IConfigurationHelper> _configurationHelperMock;

        private int _requestCount;

        [TestInitialize]
        public void Setup()
        {
            _configurationHelperMock = new Mock<IConfigurationHelper>();
            _configurationHelperMock.Setup(x => x.GetServer()).Returns(IPAddress.Parse(Constants.Localhost));
            _configurationHelperMock.Setup(x => x.GetProtocol()).Returns(Constants.Http);
        }


        [TestMethod]
        public void Run_100RequestsSent_100RequestsReceivedOnServer()
        {
            _configurationHelperMock.Setup(x => x.GetPort()).Returns(12350);

            var pageLoaderMock = new Mock<IPageLoader>();
            pageLoaderMock.Setup(x => x.LoadContentFromLocalFile(It.IsAny<string>()))
                .Callback(() => { _requestCount++; });

            var socketWebServer = new SocketWebServer(pageLoaderMock.Object,
                new CustomTcpListener(_configurationHelperMock.Object));

            socketWebServer.Run();

            for (var i = 0; i < 100; i++)
            {
                SendRequestWithResponse();
            }

            Assert.AreEqual(100, _requestCount);
        }

        [TestMethod]
        public void Run_2LongRequestsSentSimultaneously_ProcessedSimultaneously()
        {
            _configurationHelperMock.Setup(x => x.GetPort()).Returns(12351);

            var longTaskOnServerTime = 4000;
            var numberOfRequests = 2;
            var toleratedCompletionTime = longTaskOnServerTime*1.4;

            var pageLoaderMock = new Mock<IPageLoader>();
            pageLoaderMock.Setup(x => x.LoadContentFromLocalFile(It.IsAny<string>())).Callback(() =>
            {
                Thread.Sleep(longTaskOnServerTime);
                _requestCount++;
            });

            var socketWebServer = new SocketWebServer(pageLoaderMock.Object,
                new CustomTcpListener(_configurationHelperMock.Object));

            socketWebServer.Run();

            var processingStarted = DateTime.Now;

            // Send asynchronously requests
            for (var i = 0; i < numberOfRequests; i++)
            {
                SendRequestWithoutResponse();
            }

            // Wait for all of them to be processed by the server
            while (_requestCount != numberOfRequests)
            {
                Thread.Sleep(500);
            }

            var processingFinished = DateTime.Now;

            var approximateExecutionTime = (processingFinished - processingStarted).TotalSeconds;
            Assert.AreEqual(numberOfRequests, _requestCount);
            Assert.IsTrue(approximateExecutionTime < toleratedCompletionTime/1000);
        }

        private void SendRequestWithResponse()
        {
            using (var client = new TcpClient(Constants.Localhost, _configurationHelperMock.Object.GetPort()))
            {
                using (var stream = client.GetStream())
                {
                    var reader = new StreamReader(stream);
                    var writer = new StreamWriter(stream) {AutoFlush = true};

                    var request = new SimpleProtocolClientRequest
                    {
                        Url = "index.html"
                    };

                    writer.WriteLine(JsonConvert.SerializeObject(request));
                    reader.ReadToEnd();
                }
            }
        }

        private void SendRequestWithoutResponse()
        {
            using (var client = new TcpClient(Constants.Localhost, _configurationHelperMock.Object.GetPort()))
            {
                using (var stream = client.GetStream())
                {
                    var writer = new StreamWriter(stream) {AutoFlush = true};

                    var request = new SimpleProtocolClientRequest
                    {
                        Url = "index.html"
                    };

                    writer.WriteLine(JsonConvert.SerializeObject(request));
                }
            }
        }
    }
}
