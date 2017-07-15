using System;
using System.Configuration;
using System.Net;
using Helpers;
using Helpers.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ConfigurationHelperTests
    {
        private readonly ConfigurationHelper _configurationHelper = new ConfigurationHelper();
        
        [TestMethod]
        public void GetProtocol_CorrectHttpProtocolDefined_ProtocolReturned()
        {
            ConfigurationManager.AppSettings["Protocol"] = Constants.Http;

            var result = _configurationHelper.GetProtocol();

            Assert.IsNotNull(result);
            Assert.AreEqual(Constants.Http, result);
        }

        [TestMethod]
        public void GetProtocol_CorrectHttpsProtocolDefined_ProtocolReturned()
        {
            ConfigurationManager.AppSettings["Protocol"] = Constants.Https;

            var result = _configurationHelper.GetProtocol();

            Assert.IsNotNull(result);
            Assert.AreEqual(Constants.Https, result);
        }

        [TestMethod]
        public void GetProtocol_NullProtocolDefined_Exception()
        {
            ConfigurationManager.AppSettings["Protocol"] = (string) null;

            try
            {
                _configurationHelper.GetProtocol();
                Assert.Fail("Exception expected");
            }
            catch(Exception ex)
            {
                Assert.AreEqual(ErrorMessages.MissingProtocol, ex.Message);
            }
        }

        [TestMethod]
        public void GetProtocol_EmptyProtocolDefined_Exception()
        {
            ConfigurationManager.AppSettings["Protocol"] = string.Empty;

            try
            {
                _configurationHelper.GetProtocol();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.MissingProtocol, ex.Message);
            }
        }

        [TestMethod]
        public void GetProtocol_WhitespaceProtocolDefined_Exception()
        {
            ConfigurationManager.AppSettings["Protocol"] = " ";

            try
            {
                _configurationHelper.GetProtocol();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.WrongProtocol, ex.Message);
            }
        }

        [TestMethod]
        public void GetProtocol_WrongProtocolDefined_Exception()
        {
            ConfigurationManager.AppSettings["Protocol"] = "random";

            try
            {
                _configurationHelper.GetProtocol();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.WrongProtocol, ex.Message);
            }
        }

        [TestMethod]
        public void GetServer_CorrectServerDefined_ServerReturned()
        {
            ConfigurationManager.AppSettings["Server"] = Constants.Localhost;

            var result = _configurationHelper.GetServer();

            Assert.IsNotNull(result);
            Assert.AreEqual(IPAddress.Parse(Constants.Localhost), result);
        }

        [TestMethod]
        public void GetServer_NullServerDefined_Exception()
        {
            ConfigurationManager.AppSettings["Server"] = (string)null;

            try
            {
                _configurationHelper.GetServer();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.MissingServer, ex.Message);
            }
        }


        [TestMethod]
        public void GetServer_EmptyServerDefined_Exception()
        {
            ConfigurationManager.AppSettings["Server"] = string.Empty;

            try
            {
                _configurationHelper.GetServer();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.MissingServer, ex.Message);
            }
        }

        [TestMethod]
        public void GetServer_WhitespaceServerDefined_Exception()
        {
            ConfigurationManager.AppSettings["Server"] = " ";

            try
            {
                _configurationHelper.GetServer();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.WrongServer, ex.Message);
            }
        }

        [TestMethod]
        public void GetServer_WrongServerDefined_Exception()
        {
            ConfigurationManager.AppSettings["Server"] = "nonsense";

            try
            {
                _configurationHelper.GetServer();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.WrongServer, ex.Message);
            }
        }

        [TestMethod]
        public void GetPort_CorrectPortDefined_PortReturned()
        {
            ConfigurationManager.AppSettings["Port"] = "123456";

            var result = _configurationHelper.GetPort();

            Assert.IsNotNull(result);
            Assert.AreEqual(123456, result);
        }
        
        [TestMethod]
        public void GetPort_NullPortDefined_Exception()
        {
            ConfigurationManager.AppSettings["Port"] = (string)null;

            try
            {
                _configurationHelper.GetPort();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.MissingPort, ex.Message);
            }
        }

        [TestMethod]
        public void GetPort_EmptyPortDefined_Exception()
        {
            ConfigurationManager.AppSettings["Port"] = string.Empty;

            try
            {
                _configurationHelper.GetPort();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.MissingPort, ex.Message);
            }
        }

        [TestMethod]
        public void GetPort_WhitespacePortDefined_Exception()
        {
            ConfigurationManager.AppSettings["Port"] = " ";

            try
            {
                _configurationHelper.GetPort();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.WrongPort, ex.Message);
            }
        }

        [TestMethod]
        public void GetPort_NonnumericPortDefined_Exception()
        {
            ConfigurationManager.AppSettings["Port"] = "localhost";

            try
            {
                _configurationHelper.GetPort();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.WrongPort, ex.Message);
            }
        }

        [TestMethod]
        public void GetPort_7digitsPortDefined_Exception()
        {
            ConfigurationManager.AppSettings["Port"] = "1234567";

            try
            {
                _configurationHelper.GetPort();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ErrorMessages.WrongPort, ex.Message);
            }
        }

    }
}