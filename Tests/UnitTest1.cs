using System;
using System.Configuration;
using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetHost_CorrectHttpHost_AddressReturned()
        {
            var input = "http://localhost:12345/";
            ConfigurationManager.AppSettings["HostName"] = input;

            var _configurationHelper = new ConfigurationHelper();

            var result = _configurationHelper.GetHost();

            Assert.IsNotNull(result);
            Assert.AreEqual(input, result);
        }
    }
}
